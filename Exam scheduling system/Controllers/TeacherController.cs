using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using ExamSchedulingSystem.Models;
using System.Collections.Generic;
using ExamSchedulingSystem.Data;
using Microsoft.EntityFrameworkCore;

public class TeacherController : Controller
{
    private readonly ApplicationDbContext _context;

    public TeacherController(ApplicationDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public IActionResult RequestChange()
    {
        var teacherId = HttpContext.Session.GetString("UserId");

       
        var examReservations = _context.ExamReservations
            .Join(
                _context.Lectures,
                reservation => reservation.CourseId,
                lecture => lecture.CourseId,
                (reservation, lecture) => new { reservation, lecture }
            )
            .Where(x => x.lecture.UserId == teacherId && x.lecture.UserRole == "Teacher")
            .Select(x => x.reservation)
            .Distinct()
            .ToList();

        var model = new RequestChangeViewModel
        {
            ExamReservations = examReservations
        };

        return View(model);
    }

   
    [HttpGet]
    public IActionResult EnterRequestedChange(int reservationId)
    {
        var teacherName = HttpContext.Session.GetString("UserName");

        var reservation = _context.ExamReservations
            .Where(r => r.ReservationId == reservationId)
            .Select(r => new { r.CourseName, r.ExamDate, r.StartTime, r.EndTime, r.RoomId })
            .FirstOrDefault();

        if (reservation == null)
        {
            return NotFound("Reservation not found.");
        }

        var model = new RequestChangeViewModel
        {
            ReservationId = reservationId,
            ExamName = reservation.CourseName,
            ExamDate = reservation.ExamDate,
            StartTime = reservation.StartTime,
            EndTime = reservation.EndTime,
            Room = reservation.RoomId,
            RequestHistory = _context.ChangeRequests
                .Where(cr => cr.TeacherName == teacherName && cr.ReservationId == reservationId) 
                .Select(cr => new ChangeRequestViewModel
                {
                    RequestId = cr.RequestId,
                    ExamName = cr.Reservation.CourseName,
                    RequestText = cr.RequestText,
                    ExamDate = cr.Reservation.ExamDate,
                    StartTime = cr.Reservation.StartTime,
                    EndTime = cr.Reservation.EndTime,
                    Status = cr.IsAccepted.HasValue ? (cr.IsAccepted.Value ? "Accepted" : "Rejected") : "Pending",
                    CoordinatorName = cr.CoordinatorName
                })
                .ToList() ?? new List<ChangeRequestViewModel>()
        };

        return View("EnterRequestedChange", model);
    }




    [HttpPost]
    public IActionResult SubmitChangeRequest(RequestChangeViewModel model)
    {
        if (ModelState.IsValid)
        {
            var teacherName = HttpContext.Session.GetString("UserName");

           
            var reservation = _context.ExamReservations
                .Where(r => r.ReservationId == model.ReservationId)
                .Select(r => new { r.CoordinatorId })
                .FirstOrDefault();

            if (reservation != null)
            {
                
                var coordinatorName = _context.Users
                    .Where(u => u.UserId == reservation.CoordinatorId)
                    .Select(u => u.Name)
                    .FirstOrDefault();

                if (coordinatorName != null)
                {
                    var changeRequest = new ChangeRequest
                    {
                        ReservationId = model.ReservationId,
                        TeacherName = teacherName,
                        CoordinatorName = coordinatorName,
                        RequestText = model.RequestText,
                        IsAccepted = null
                    };
                    _context.ChangeRequests.Add(changeRequest);
                    _context.SaveChanges();

                    return RedirectToAction("RequestChange");
                }

                ModelState.AddModelError("", "Coordinator not found.");
            }
            else
            {
                ModelState.AddModelError("", "Reservation not found.");
            }
        }
        return View("EnterRequestedChange", model);
    }
    [HttpGet]
    public IActionResult ManageReservations()
    {
        var teacherName = HttpContext.Session.GetString("UserName");
        Console.WriteLine($"Teacher Name from Session: {teacherName}");

        
        var latestRequests = _context.ChangeRequests
            .Where(cr => cr.TeacherName == teacherName)
            .Include(cr => cr.Reservation) 
            .GroupBy(cr => cr.ReservationId)
            .Select(g => g.OrderByDescending(cr => cr.RequestId).FirstOrDefault())
            .ToList();

        Console.WriteLine($"Latest Requests Count: {latestRequests.Count}");
        foreach (var request in latestRequests)
        {
            Console.WriteLine($"RequestId: {request?.RequestId}, ReservationId: {request?.ReservationId}, IsAccepted: {request?.IsAccepted}, CourseName: {request?.Reservation?.CourseName}");
        }

       
        var acceptedReservations = latestRequests
            .Where(cr => cr != null && cr.IsAccepted == true && cr.Reservation != null)
            .Select(cr => cr.Reservation) 
            .Distinct()
            .ToList();

        Console.WriteLine($"Accepted Reservations Count: " + acceptedReservations.Count);
        foreach (var reservation in acceptedReservations)
        {
            if (reservation != null)
            {
                Console.WriteLine($"ReservationId: {reservation.ReservationId}, CourseName: {reservation.CourseName}");
            }
            else
            {
                Console.WriteLine("Encountered a null reservation entry.");
            }
        }

        return View(acceptedReservations);
    }
    [HttpGet]
    public IActionResult TeacherNotifications()
    {
        
        return View();
    }

    [HttpGet]
    public IActionResult EditReservation(int reservationId)
    {
        var reservation = _context.ExamReservations.Find(reservationId);
        if (reservation == null)
        {
            return NotFound("Reservation not found.");
        }

       
        var courseName = _context.Courses
            .Where(c => c.CourseId == reservation.CourseId)
            .Select(c => c.CourseName)
            .FirstOrDefault();

      
        ViewBag.CourseName = courseName;

        var model = new ReserveExamViewModel
        {
            ReservationId = reservation.ReservationId,
            SelectedCourseId = reservation.CourseId,
            SelectedExamType = reservation.ExamType,
            SelectedDate = reservation.ExamDate,
            SelectedTimeSlot = $"{reservation.StartTime:hh\\:mm} - {reservation.EndTime:hh\\:mm}",
            SelectedRoomId = reservation.RoomId
        };

        return View(model);
    }
    [HttpPost]
    public IActionResult EditReservation(ReserveExamViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }

            var reservation = _context.ExamReservations.FirstOrDefault(r => r.ReservationId == model.ReservationId);
            if (reservation == null)
            {
                return NotFound("Reservation not found.");
            }

           

            
            var existingSchedule = _context.ExamSchedules.FirstOrDefault(es =>
                es.CourseId == reservation.CourseId &&
                es.ExamDate == reservation.ExamDate &&
                es.StartTime == reservation.StartTime &&
                es.EndTime == reservation.EndTime);

            if (existingSchedule == null)
            {
                return NotFound("Exam schedule not found for the original reservation.");
            }

           
            reservation.ExamDate = model.SelectedDate ?? DateTime.Today;

            var timeParts = model.SelectedTimeSlot?.Split('-');
            if (timeParts != null && timeParts.Length == 2 &&
                TimeSpan.TryParse(timeParts[0].Trim(), out var startTime) &&
                TimeSpan.TryParse(timeParts[1].Trim(), out var endTime))
            {
                reservation.StartTime = startTime;
                reservation.EndTime = endTime;
            }
            else
            {
                ModelState.AddModelError("", "Invalid time slot selected.");
                return View(model);
            }

            reservation.RoomId = model.SelectedRoomId;

         
            existingSchedule.ExamDate = reservation.ExamDate;
            existingSchedule.StartTime = reservation.StartTime;
            existingSchedule.EndTime = reservation.EndTime;
            existingSchedule.Place = reservation.RoomId;

            _context.SaveChanges();

            return RedirectToAction("ManageReservations", "Teacher");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred while updating the reservation.");
            return View(model);
        }
    }


}