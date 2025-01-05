using ExamSchedulingSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamSchedulingSystem.Models
{
    [Table("Lectures")]
    public class Lecture
    {
        [Key]
        [Column("lecture_id")]
        public int LectureId { get; set; }

        [Column("course_id")]
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }

       
        [Column("slot_id")]
        public int SlotId { get; set; }

        [ForeignKey("SlotId")]
        public TimeSlot TimeSlot { get; set; }

        [Column("room_id")]
        [StringLength(10)]
        public string RoomId { get; set; }

        [ForeignKey("RoomId")]
        public ClassRoom Classroom { get; set; } // Ensure you have a Classroom model

        [Column("user_id")]
        [StringLength(450)] // Ensure consistency with your database's UserId column
        public string UserId { get; set; } // Stores the coordinator/teacher's ID

        [Column("user_role")]
        [StringLength(50)]
        public string UserRole { get; set; } 

        [Column("number_of_students")]
        public int NumberOfStudents { get; set; }

        [Column("section_id")]
        public int SectionId { get; set; }
    }
}
