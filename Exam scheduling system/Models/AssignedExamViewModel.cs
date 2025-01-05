namespace ExamSchedulingSystem.Models
{
    public class AssignedExamViewModel
    {
        public int ReservationId { get; set; }
        public string ExamName { get; set; }
        public DateTime ExamDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string CoordinatorName { get; set; }
    }

}
