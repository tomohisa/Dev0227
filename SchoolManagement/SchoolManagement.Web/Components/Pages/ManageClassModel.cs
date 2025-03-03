using SchoolManagement.Domain;

namespace SchoolManagement.Web.Components.Pages;

public class ManageClassModel
{
    public Guid ClassId { get; set; }
    public string ClassName { get; set; } = "";
    public TeacherQuery.TeacherRecord? CurrentTeacher { get; set; }
    public StudentQuery.StudentRecord[]? ClassStudents { get; set; }
}