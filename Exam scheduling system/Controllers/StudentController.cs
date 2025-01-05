using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using ExamSchedulingSystem.Models;
using ExamSchedulingSystem.Data;
namespace ExamSchedulingSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult SearchCourseByName()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SearchCourseResults(SearchCourseByNameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("SearchCourseByName", model); 
            }

            var exams = _context.ExamSchedules
                .Where(e => e.CourseName.Contains(model.courseName))
                .Select(e => new
                {
                    e.CourseName,
                    e.ExamDate,
                    e.StartTime,
                    e.EndTime,
                    e.Place
                })
                .ToList();

            if (!exams.Any())
            {
                ViewBag.Message = "No exams found for the specified course name.";
            }

            return View("SearchCourseResults", exams);
        }
   
        
        [HttpGet]
        public IActionResult StudentNotifications()
        {
            var studentId = HttpContext.Session.GetString("UserId");
            var unreadCount = _context.Notifications
           .Where(n => n.RecipientId == studentId && !n.IsRead)
           .Count();

            
            ViewBag.UnreadNotificationCount = unreadCount;

            var unreadNotifications = _context.Notifications
        .Where(n => n.RecipientId == studentId && !n.IsRead)
        .ToList();

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
            }
            _context.SaveChanges();

            var notifications = _context.Notifications
                .Where(n => n.RecipientId == studentId)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();

            ViewBag.UnreadNotificationCount = 0;
            return View(notifications);
        }

        [HttpPost]
        public IActionResult DeleteNotification(int notificationId)
        {
            var notification = _context.Notifications.Find(notificationId);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                _context.SaveChanges();
            }
            return RedirectToAction("StudentNotifications");
        }
    }
}