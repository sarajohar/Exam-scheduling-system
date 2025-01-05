using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace ExamSchedulingSystem.Models
{
    public class ClassRoom
    {
        [Key]
        [Column("room_id")]
        [StringLength(10)]
        [Required]
        public string RoomId { get; set; }

        [Column("capacity")]
        public int Capacity { get; set; }



    }
}
