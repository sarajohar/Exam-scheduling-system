using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamSchedulingSystem.Models
{
    [Table("TimeSlots")]
    public class TimeSlot
    {
        [Key]
        [Column("slot_id")]
        public int SlotId { get; set; }

        [Column("day")]
        [StringLength(10)]
        public string Day { get; set; }

        [Column("start_time")]
        public TimeSpan StartTime { get; set; }

        [Column("end_time")]
        public TimeSpan EndTime { get; set; }
    }
}
