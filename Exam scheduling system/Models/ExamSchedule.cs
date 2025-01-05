using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamSchedulingSystem.Models
{
    [Table("ExamSchedules")]
    public class ExamSchedule
    {
        [Key]
        [Column("schedule_id")]
        public int ScheduleId { get; set; } 

        [Column("course_id")]
        [Required]
        public int CourseId { get; set; } 

        [ForeignKey("CourseId")]
        public Course Course { get; set; } 
        [Column("course_name")]
        [StringLength(60)]
        public string CourseName { get; set; } 

        [Column("exam_date")]
        [Required]
        public DateTime ExamDate { get; set; } 

        [Column("start_time")]
        [Required]
        public TimeSpan StartTime { get; set; } 

        [Column("end_time")]
        [Required]
        public TimeSpan EndTime { get; set; } 

        [Column("place")]
        [StringLength(255)]
        public string Place { get; set; } 
    }
}
