using System.ComponentModel.DataAnnotations;


namespace ExamSchedulingSystem.Models
{

    

    public enum UserRole
    {
        Student,
        Faculty,
        Admin
    }

    public enum FacultyRole
    {
        Coordinator,
        Teacher,
        Invigilator
    }
    public class User
    {
        [Key]
        [Required(ErrorMessage = "ID is required.")]
        [CustomIdValidation(ErrorMessage = "Invalid ID length.")]
        public string? UserId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string ?Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [RegularExpression(@"^[0-9]+@std\.hu\.edu\.jo$", ErrorMessage = "Invalid email format. Must be like 2143217@std.hu.edu.jo.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string ?Password { get; set; }

        public UserRole Role { get; set; } 
        
        public FacultyRole? FacultyRole { get; set; }
        

        
        public ICollection<Coordinator> ?Coordinators { get; set; }
        public ICollection<Teacher> ?Teachers { get; set; }
        public ICollection<Invigilator>? Invigilators { get; set; }
        public Student ?Student { get; set; }

    }
}