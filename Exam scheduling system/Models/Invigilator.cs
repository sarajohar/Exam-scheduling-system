using System.ComponentModel.DataAnnotations;

namespace ExamSchedulingSystem.Models
{
    public class Invigilator
    {
        [Key]
        public string UserId { get; set; }         

        public User  User { get; set; }

        
    }

}
