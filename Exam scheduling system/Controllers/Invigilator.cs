using ExamSchedulingSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamSchedulingSystem.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExamSchedulingSystem.Controllers
{
    public class Invigilator : Controller
    {
        private readonly ApplicationDbContext _context;

        public Invigilator(ApplicationDbContext context)
        {
            _context = context;
        }
       
        [HttpGet]
        public IActionResult ViewAssignedExams()
        {
            string invigilatorName = HttpContext.Session.GetString("UserName");
            var assignedExams = _context.ExamReservations
                .Where(r => r.InvigilatorName == invigilatorName)
                .Select(r => new AssignedExamViewModel
                {
                    ReservationId = r.ReservationId,
                    ExamName = r.CourseName,
                    ExamDate = r.ExamDate,
                    StartTime = r.StartTime,
                    EndTime = r.EndTime,
                    CoordinatorName = r.Coordinator.Name
                })
                .ToList();

            return View(assignedExams);
        }
       
        [HttpGet]
        public IActionResult EnterExcuse(int reservationId)
        {
            var invigilatorName = HttpContext.Session.GetString("UserName");

            var model = new EnterExcuseViewModel
            {
                ReservationId = reservationId,
                ExcuseHistory = _context.Excuses
                    .Where(e => e.InvigilatorName == invigilatorName && e.ReservationId == reservationId) 
                    .Select(e => new ExcuseViewModel
                    {
                        ExcuseId = e.ExcuseId,
                        ExamName = e.Reservation.CourseName,
                        ExcuseText = e.ExcuseText,
                        ExamDate = e.Reservation.ExamDate,
                        StartTime = e.Reservation.StartTime,
                        EndTime = e.Reservation.EndTime,
                        Status = e.IsAccepted.HasValue ? (e.IsAccepted.Value ? "Accepted" : "Rejected") : "Pending",
                        CoordinatorName = e.CoordinatorName
                    })
                    .ToList() ?? new List<ExcuseViewModel>()
            };

            return View(model);
        }


        [HttpPost]
        public IActionResult EnterExcuse(EnterExcuseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var invigilatorName = HttpContext.Session.GetString("UserName");

              
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
                        var excuse = new Excuse
                        {
                            ReservationId = model.ReservationId,
                            InvigilatorName = invigilatorName,
                            ExcuseText = model.ExcuseText,
                            IsAccepted = null, 
                            CoordinatorName = coordinatorName 
                        };

                      

                        _context.Excuses.Add(excuse);
                        _context.SaveChanges();

                        return RedirectToAction("ViewAssignedExams");
                    }

                    ModelState.AddModelError("", "Coordinator not found for the reservation.");
                }
                else
                {
                    ModelState.AddModelError("", "Reservation not found.");
                }
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult InvigilatorNotifications()
        {
            
            return View();
        }

    }
}
