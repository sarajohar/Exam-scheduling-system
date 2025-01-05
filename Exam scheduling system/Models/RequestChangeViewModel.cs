using System.ComponentModel.DataAnnotations;

namespace ExamSchedulingSystem.Models
{
    public class RequestChangeViewModel
    {
        public int ReservationId { get; set; }
        public string ? ExamName { get; set; }
        public DateTime? ExamDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string ?Room { get; set; }
        [Required(ErrorMessage = "Please enter a requested change.")]
        public string RequestText { get; set; }
        public List<ExamReservation> ?ExamReservations { get; set; }
        public List<ChangeRequestViewModel> ?RequestHistory { get; set; }
    }

}
