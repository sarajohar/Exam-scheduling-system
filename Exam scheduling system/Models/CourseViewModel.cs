 using System.ComponentModel.DataAnnotations;
namespace ExamSchedulingSystem.Models
{
   
    public class CourseViewModel
    {
        [Required]
        public int CourseId { get; set; } 

        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Course name must contain only letters.")]
        public string CourseName { get; set; }

        public int? PrerequisiteId { get; set; } 

        [Required]
        public string CourseDepartment { get; set; }
    }

}
