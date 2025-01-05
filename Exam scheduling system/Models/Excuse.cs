using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamSchedulingSystem.Models
{
    public class Excuse
    {
        [Key]
        public int ExcuseId { get; set; }

        [Required]
        public int ReservationId { get; set; } 

        [ForeignKey("ReservationId")]
        public ExamReservation Reservation { get; set; }

        [Required]
        public string InvigilatorName { get; set; } 
        [Required]
        public string CoordinatorName { get; set; }

        [Required]
        public string ExcuseText { get; set; } 

        public bool? IsAccepted { get; set; } 

       
    }
}
