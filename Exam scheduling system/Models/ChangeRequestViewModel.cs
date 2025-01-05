namespace ExamSchedulingSystem.Models
{
    public class ChangeRequestViewModel
    {
        public int RequestId { get; set; }
        public string TeacherName { get; set; }
        public string ExamName { get; set; }
        public DateTime ExamDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string RequestText { get; set; }
        public string CoordinatorName { get; set; }

        public string Status { get; set; }  
        public bool? IsAccepted { get; set; }
    }

}
