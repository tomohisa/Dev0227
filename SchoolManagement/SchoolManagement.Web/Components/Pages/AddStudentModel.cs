using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Web.Components.Pages;

public class AddStudentModel
{
    public Guid ClassId { get; set; }

    [Required(ErrorMessage = "Student is required")]
    public string StudentId { get; set; } = "";

    public string? Error { get; set; }
}