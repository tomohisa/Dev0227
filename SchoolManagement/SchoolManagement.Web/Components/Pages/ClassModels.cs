using System;
using System.ComponentModel.DataAnnotations;
using SchoolManagement.Domain;

namespace SchoolManagement.Web.Components.Pages
{
    public class ClassModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Class code is required")]
        public string ClassCode { get; set; } = "";

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = "";

        public string? Error { get; set; }
    }

    public class EditClassModel
    {
        public Guid ClassId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Class code is required")]
        public string ClassCode { get; set; } = "";

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = "";

        public string? Error { get; set; }
    }

    public class ManageClassModel
    {
        public Guid ClassId { get; set; }
        public string ClassName { get; set; } = "";
        public TeacherQuery.TeacherRecord? CurrentTeacher { get; set; }
        public StudentQuery.StudentRecord[]? ClassStudents { get; set; }
    }

    public class AssignTeacherModel
    {
        public Guid ClassId { get; set; }

        [Required(ErrorMessage = "Teacher is required")]
        public string TeacherId { get; set; } = "";

        public string? Error { get; set; }
    }

    public class AddStudentModel
    {
        public Guid ClassId { get; set; }

        [Required(ErrorMessage = "Student is required")]
        public string StudentId { get; set; } = "";

        public string? Error { get; set; }
    }
}
