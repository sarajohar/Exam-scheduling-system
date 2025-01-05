//using Examschedulingsystem.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamSchedulingSystem.Models
{
    public enum ExamType
    {
        First,
        Second,
        Mid
    }
    [Table("ExamReservations")]
    public class ExamReservation
    {
        [Key]
        [Column("reservation_id")]
        public int ReservationId { get; set; } // Primary Key

        [Column("course_id")]
        [Required]
        public int CourseId { get; set; } 

        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        [Column("course_name")]
        [StringLength(60)]
        public string CourseName { get; set; } 
        [Column("exam_type")]
        public ExamType ExamType { get; set; }
        [Column("exam_date")]
        [Required]
        public DateTime ExamDate { get; set; } 

        [Column("start_time")]
        [Required]
        public TimeSpan StartTime { get; set; } 

        [Column("end_time")]
        [Required]
        public TimeSpan EndTime { get; set; } 

      

        [Column("room_id")]
        [Required]
        [StringLength(10)]
        public string RoomId { get; set; } 

        [ForeignKey("RoomId")]
        public ClassRoom Room { get; set; } 

        [Column("coordinator_id")]
        public string CoordinatorId { get; set; }

        [ForeignKey("CoordinatorId")]
        public User Coordinator { get; set; } 

        [Column("invigilator_name")]
        [StringLength(100)]
        public string InvigilatorName { get; set; }
    }
}
