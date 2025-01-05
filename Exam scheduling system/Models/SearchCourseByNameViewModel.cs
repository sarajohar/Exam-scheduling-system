using System.ComponentModel.DataAnnotations;

namespace ExamSchedulingSystem.Models
{
    public class SearchCourseByNameViewModel
    {
        [Required(ErrorMessage ="Course Name Field is required")]
        public string courseName { get; set; }
    }
}
