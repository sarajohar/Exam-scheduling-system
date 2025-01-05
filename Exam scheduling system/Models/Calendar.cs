using System;
using System.ComponentModel.DataAnnotations;

namespace ExamSchedulingSystem.Models
{
    public class Calendar
    {
        [Key]
        public int CalendarId { get; set; } // Primary Key

        [Required]
        [StringLength(10)]
        public string ExamType { get; set; } // Exam type (First, Second, Mid)

        [Required]
        public DateTime StartDate { get; set; } // Start of the exam period

        [Required]
        public DateTime EndDate { get; set; } // End of the exam period
    }
}
