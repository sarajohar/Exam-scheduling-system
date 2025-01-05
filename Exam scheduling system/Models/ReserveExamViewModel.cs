
using ExamSchedulingSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class ReserveExamViewModel
{
    public int ReservationId { get; set; }
    public List<TimeSlot> AvailableTimeSlots { get; set; } = new List<TimeSlot>();
    public List<ClassRoom> AvailableRooms { get; set; } = new List<ClassRoom>();
    public List<Course> Courses { get; set; } = new List<Course>();

    [Required]
    public int SelectedCourseId { get; set; } = 0; 

    [Required]
    public ExamType? SelectedExamType { get; set; } 

    [Required]
    [DataType(DataType.Date)]
    public DateTime? SelectedDate { get; set; } 

    [Required]
    public string SelectedTimeSlot { get; set; } = string.Empty; 

    [Required]
    public string SelectedRoomId { get; set; } = string.Empty; 

    public string ?SelectedInvigilatorName { get; set; }
}
