using System;
using System.ComponentModel.DataAnnotations;

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
}
