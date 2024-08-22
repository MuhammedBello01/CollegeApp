using System.ComponentModel.DataAnnotations;
using CollegeApp.Models.Validators;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CollegeApp.Models
{
    public class StudentDto
    {
        [ValidateNever]//ensures this field is never validated
        public int Id { get; set; }
        [Required(ErrorMessage = "Student name is required")]
        [StringLength(30)]
        public string? StudentName { get; set; }
        [EmailAddress(ErrorMessage = "Please enter valid email address")]
        public string? Email { get; set; }
        [Range(10, 20)]
        public int Age { get; set;  }
        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }
        [DateCheck]
        public DateTime AdmissionDate { get; set;  }
    }
}
