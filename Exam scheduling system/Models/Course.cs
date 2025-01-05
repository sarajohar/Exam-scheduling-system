using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamSchedulingSystem.Models
{
    [Table("Courses")] 

    public class Course
    {
        [Key]
        [Column("course_id")]
        public int CourseId { get; set; } 


        [Column("course_name")]
        [StringLength(60)]
        public string? CourseName { get; set; } 

        [Column("prerequest_id")]
        public int? PrerequestId { get; set; } 

        [Column("course_department")]
        [StringLength(50)]
        public string ?CourseDepartment { get; set; } 
        [Column("year_level")]
        public int YearLevel { get; set; }
    }
}