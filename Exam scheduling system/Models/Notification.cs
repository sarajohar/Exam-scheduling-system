using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamSchedulingSystem.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        public string RecipientId { get; set; } 

        [ForeignKey("RecipientId")]
        public User User { get; set; }

        public string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;
    }

    
}

