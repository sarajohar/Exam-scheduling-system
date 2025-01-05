using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamSchedulingSystem.Models
{
    public class ChangeRequest
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        public int ReservationId { get; set; }

        [ForeignKey("ReservationId")]
        public ExamReservation Reservation { get; set; }

        [Required]
        public string TeacherName { get; set; } 


        [Required]
        public string CoordinatorName { get; set; }

        [Required]
        public string RequestText { get; set; } 

        public bool? IsAccepted { get; set; } 
    }
}
