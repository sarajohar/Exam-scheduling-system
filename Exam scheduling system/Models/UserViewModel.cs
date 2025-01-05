
using System.ComponentModel.DataAnnotations;
namespace ExamSchedulingSystem.Models
{
    public class UserViewModel
    {
        [Required]
        public string UserId { get; set; }

        public string GetValidationError()
        {
            if (Role == UserRole.Faculty && UserId.Length != 5)
                return "Faculty ID must be 5 characters for Faculty role.";

            if (Role == UserRole.Student && UserId.Length != 7)
                return "Student ID must be 7 characters for Student role.";

            return null;
        }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public UserRole Role { get; set; } 

        public FacultyRole? FacultyRole { get; set; } 
    }
}