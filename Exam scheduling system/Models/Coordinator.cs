﻿using System.ComponentModel.DataAnnotations;

namespace ExamSchedulingSystem.Models
{
    public class Coordinator
    {
        [Key]
        public string  UserId { get; set; }         
        public User User { get; set; }
    }
}
