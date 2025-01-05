
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    namespace ExamSchedulingSystem.Models
    {
        public class CalendarViewModel
        {
            [Required]
            public DateTime FirstExamStartDate { get; set; }
            [Required]
            public DateTime FirstExamEndDate { get; set; }

            [Required]
            public DateTime SecondExamStartDate { get; set; }
            [Required]
            public DateTime SecondExamEndDate { get; set; }

            [Required]
            public DateTime MidExamStartDate { get; set; }
            [Required]
            public DateTime MidExamEndDate { get; set; }
        }
    }


