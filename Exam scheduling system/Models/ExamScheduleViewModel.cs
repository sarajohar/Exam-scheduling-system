namespace ExamSchedulingSystem.Models
{
    public class ExamScheduleViewModel
    {
        public string ExamName { get; set; }
        public DateTime ExamDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Room { get; set; }
    }
}
