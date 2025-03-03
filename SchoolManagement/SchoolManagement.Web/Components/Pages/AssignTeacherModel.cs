using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Web.Components.Pages;

public class AssignTeacherModel
{
    public Guid ClassId { get; set; }

    [Required(ErrorMessage = "Teacher is required")]
    public string TeacherId { get; set; } = "";

    public string? Error { get; set; }
}