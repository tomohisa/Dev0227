using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SchoolManagement.Domain;

namespace SchoolManagement.Web.Components.Pages
{
    public class ClassesBase : ComponentBase
    {
        [Inject] protected ClassApiClient ClassApi { get; set; } = default!;
        [Inject] protected StudentApiClient StudentApi { get; set; } = default!;
        [Inject] protected TeacherApiClient TeacherApi { get; set; } = default!;
        [Inject] protected IJSRuntime JsRuntime { get; set; } = default!;

        protected ClassQuery.ClassRecord[]? classes;
        protected TeacherQuery.TeacherRecord[]? teachers;
        protected StudentQuery.StudentRecord[]? students;
        protected TeacherQuery.TeacherRecord[]? availableTeachers;
        protected StudentQuery.StudentRecord[]? availableStudents;
        protected string nameFilter = "";
        protected string classCodeFilter = "";
        protected ClassModel classModel = new();
        protected EditClassModel editClassModel = new();
        protected ManageClassModel manageClassModel = new();
        protected AssignTeacherModel assignTeacherModel = new();
        protected AddStudentModel addStudentModel = new();
        protected Dictionary<Guid, string> teacherNames = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadClasses();
            await LoadTeachers();
            await LoadStudents();
        }

        protected async Task LoadClasses()
        {
            classes = await ClassApi.GetClassesAsync(nameFilter, classCodeFilter);
        }

        protected async Task LoadTeachers()
        {
            teachers = await TeacherApi.GetTeachersAsync();
            teacherNames.Clear();
            if (teachers != null)
            {
                foreach (var teacher in teachers)
                {
                    teacherNames[teacher.TeacherId] = $"{teacher.Name} ({teacher.Subject})";
                }
            }
        }

        protected async Task LoadStudents()
        {
            students = await StudentApi.GetStudentsAsync();
        }

        protected string GetTeacherName(Guid teacherId)
        {
            if (teacherNames.TryGetValue(teacherId, out var name))
            {
                return name;
            }
            return "Unknown Teacher";
        }

        protected async Task ApplyFilters()
        {
            await LoadClasses();
        }

        protected async Task OpenAddClassModal()
        {
            classModel = new ClassModel();
            await JsRuntime.InvokeVoidAsync("DisplayAddClassModal", "show");
        }

        protected async Task CloseAddClassModal()
        {
            classModel = new ClassModel();
            await JsRuntime.InvokeVoidAsync("DisplayAddClassModal", "hide");
        }

        protected async Task OpenEditClassModal(ClassQuery.ClassRecord classItem)
        {
            editClassModel = new EditClassModel
            {
                ClassId = classItem.ClassId,
                Name = classItem.Name,
                ClassCode = classItem.ClassCode,
                Description = classItem.Description
            };
            await JsRuntime.InvokeVoidAsync("DisplayEditClassModal", "show");
        }

        protected async Task CloseEditClassModal()
        {
            editClassModel = new EditClassModel();
            await JsRuntime.InvokeVoidAsync("DisplayEditClassModal", "hide");
        }

        protected async Task OpenManageClassModal(ClassQuery.ClassRecord classItem)
        {
            manageClassModel = new ManageClassModel
            {
                ClassId = classItem.ClassId,
                ClassName = classItem.Name
            };

            assignTeacherModel = new AssignTeacherModel
            {
                ClassId = classItem.ClassId
            };

            addStudentModel = new AddStudentModel
            {
                ClassId = classItem.ClassId
            };

            await LoadClassTeacher(classItem);
            await LoadClassStudents(classItem.ClassId);
            await UpdateAvailableTeachers(classItem.TeacherId);
            await UpdateAvailableStudents(classItem.StudentIds);
            await JsRuntime.InvokeVoidAsync("DisplayManageClassModal", "show");
        }

        protected async Task CloseManageClassModal()
        {
            manageClassModel = new ManageClassModel();
            assignTeacherModel = new AssignTeacherModel();
            addStudentModel = new AddStudentModel();
            await JsRuntime.InvokeVoidAsync("DisplayManageClassModal", "hide");
        }

        protected async Task LoadClassTeacher(ClassQuery.ClassRecord classItem)
        {
            if (classItem.TeacherId.HasValue && teachers != null)
            {
                manageClassModel.CurrentTeacher = teachers.FirstOrDefault(t => t.TeacherId == classItem.TeacherId.Value);
            }
            else
            {
                manageClassModel.CurrentTeacher = null;
            }
        }

        protected async Task LoadClassStudents(Guid classId)
        {
            manageClassModel.ClassStudents = await StudentApi.GetStudentsByClassIdAsync(classId);
        }

        protected async Task UpdateAvailableTeachers(Guid? currentTeacherId)
        {
            if (teachers == null)
            {
                availableTeachers = Array.Empty<TeacherQuery.TeacherRecord>();
                return;
            }

            availableTeachers = currentTeacherId.HasValue
                ? teachers.Where(t => t.TeacherId != currentTeacherId.Value).ToArray()
                : teachers;
        }

        protected async Task UpdateAvailableStudents(Guid[] enrolledStudentIds)
        {
            if (students == null)
            {
                availableStudents = Array.Empty<StudentQuery.StudentRecord>();
                return;
            }

            var enrolledStudentIdSet = new HashSet<Guid>(enrolledStudentIds);
            availableStudents = students
                .Where(s => !enrolledStudentIdSet.Contains(s.StudentId) && s.ClassId == null)
                .ToArray();
        }

        protected async Task HandleAddClassSubmit()
        {
            try
            {
                await ClassApi.CreateClassAsync(
                    classModel.Name,
                    classModel.ClassCode,
                    classModel.Description);

                await LoadClasses();
                await CloseAddClassModal();
            }
            catch (Exception ex)
            {
                classModel.Error = $"Failed to add class: {ex.Message}";
            }
        }

        protected async Task HandleEditClassSubmit()
        {
            try
            {
                await ClassApi.UpdateClassAsync(
                    editClassModel.ClassId,
                    editClassModel.Name,
                    editClassModel.ClassCode,
                    editClassModel.Description);

                await LoadClasses();
                await CloseEditClassModal();
            }
            catch (Exception ex)
            {
                editClassModel.Error = $"Failed to update class: {ex.Message}";
            }
        }

        protected async Task HandleDeleteClass(Guid classId)
        {
            try
            {
                await ClassApi.DeleteClassAsync(classId);
                await LoadClasses();
            }
            catch (Exception ex)
            {
                // Show error message
                Console.Error.WriteLine($"Error deleting class: {ex.Message}");
            }
        }

        protected async Task HandleAssignTeacherSubmit()
        {
            try
            {
                if (Guid.TryParse(assignTeacherModel.TeacherId, out var teacherId))
                {
                    await ClassApi.AssignTeacherToClassAsync(assignTeacherModel.ClassId, teacherId);
                    
                    // Reload data
                    await LoadClasses();
                    var updatedClass = classes?.FirstOrDefault(c => c.ClassId == assignTeacherModel.ClassId);
                    if (updatedClass != null)
                    {
                        await LoadClassTeacher(updatedClass);
                        await UpdateAvailableTeachers(updatedClass.TeacherId);
                    }
                    
                    assignTeacherModel.TeacherId = "";
                    assignTeacherModel.Error = null;
                }
                else
                {
                    assignTeacherModel.Error = "Invalid teacher selection";
                }
            }
            catch (Exception ex)
            {
                assignTeacherModel.Error = $"Failed to assign teacher: {ex.Message}";
            }
        }

        protected async Task HandleRemoveTeacher()
        {
            try
            {
                await ClassApi.RemoveTeacherFromClassAsync(manageClassModel.ClassId);
                
                // Reload data
                await LoadClasses();
                var updatedClass = classes?.FirstOrDefault(c => c.ClassId == manageClassModel.ClassId);
                if (updatedClass != null)
                {
                    await LoadClassTeacher(updatedClass);
                    await UpdateAvailableTeachers(updatedClass.TeacherId);
                }
            }
            catch (Exception ex)
            {
                // Show error message
                Console.Error.WriteLine($"Error removing teacher: {ex.Message}");
            }
        }

        protected async Task HandleAddStudentSubmit()
        {
            try
            {
                if (Guid.TryParse(addStudentModel.StudentId, out var studentId))
                {
                    // Update both sides of the relationship
                    await ClassApi.AddStudentToClassAsync(addStudentModel.ClassId, studentId);
                    await StudentApi.AssignStudentToClassAsync(studentId, addStudentModel.ClassId);
                    
                    // Reload data
                    await LoadClasses();
                    await LoadClassStudents(addStudentModel.ClassId);
                    var updatedClass = classes?.FirstOrDefault(c => c.ClassId == addStudentModel.ClassId);
                    if (updatedClass != null)
                    {
                        await UpdateAvailableStudents(updatedClass.StudentIds);
                    }
                    
                    addStudentModel.StudentId = "";
                    addStudentModel.Error = null;
                }
                else
                {
                    addStudentModel.Error = "Invalid student selection";
                }
            }
            catch (Exception ex)
            {
                addStudentModel.Error = $"Failed to add student: {ex.Message}";
            }
        }

        protected async Task HandleRemoveStudent(Guid studentId)
        {
            try
            {
                // Update both sides of the relationship
                await ClassApi.RemoveStudentFromClassAsync(manageClassModel.ClassId, studentId);
                await StudentApi.RemoveStudentFromClassAsync(studentId);
                
                // Reload data
                await LoadClasses();
                await LoadClassStudents(manageClassModel.ClassId);
                var updatedClass = classes?.FirstOrDefault(c => c.ClassId == manageClassModel.ClassId);
                if (updatedClass != null)
                {
                    await UpdateAvailableStudents(updatedClass.StudentIds);
                }
            }
            catch (Exception ex)
            {
                // Show error message
                Console.Error.WriteLine($"Error removing student: {ex.Message}");
            }
        }
    }
}
