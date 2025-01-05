using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ExamSchedulingSystem.Models
{
    public class ExcuseViewModel
    {
        public int ExcuseId { get; set; }
        public string ExamName { get; set; }
        public string InvigilatorName { get; set; }
        public string CoordinatorName { get; set; }
        public string ExcuseText { get; set; }
        public DateTime ExamDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; } 

        public bool? IsAccepted { get; set; }
    }
}
