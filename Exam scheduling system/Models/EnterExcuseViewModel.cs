using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ExamSchedulingSystem.Models
{
    public class EnterExcuseViewModel
    {
        
            public int ReservationId { get; set; }
            [Required]
            public string ExcuseText { get; set; }
        public List<ExcuseViewModel>? ExcuseHistory { get; set; }

    }
}
