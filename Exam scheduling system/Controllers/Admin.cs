
using ExamSchedulingSystem.Data;
using ExamSchedulingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamSchedulingSystem.Controllers
{
    public class Admin : Controller
    {
        private readonly ApplicationDbContext _context;

        public Admin(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View(new UserViewModel());
        }
        [HttpPost]
        public IActionResult AddUser(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var emailRegex = new System.Text.RegularExpressions.Regex(@"^[a-z]+@hu\.edu\.jo$");
            if (!emailRegex.IsMatch(model.Email))
            {
                ModelState.AddModelError("Email", "Invalid email format. Must be like name@hu.edu.jo.");
                return View(model);
            }
            var validationError = model.GetValidationError();
            if (!string.IsNullOrEmpty(validationError))
            {
                ModelState.AddModelError(nameof(model.UserId), validationError);
                return View(model);
            }


            var existingUser = _context.Users.FirstOrDefault(u => u.UserId == model.UserId || u.Email == model.Email);
            if (existingUser != null)
            {
                if (existingUser.UserId == model.UserId)
                {
                    ModelState.AddModelError("UserId", "User ID already exists.");
                }
                if (existingUser.Email == model.Email)
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                }
                return View(model);
            }


            var newUser = new User
            {
                UserId = model.UserId,
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                Role = model.Role,
                FacultyRole = model.FacultyRole
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "User added successfully!";
            return View();
        }

        [HttpGet]

        public IActionResult AddCourse()
        {
            return View(new CourseViewModel());
        }
         
        [HttpPost]   
        public IActionResult AddCourse(CourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

           
            var existingCourse = _context.Courses.FirstOrDefault(c => c.CourseId == model.CourseId);
            if (existingCourse != null)
            {
                ModelState.AddModelError("CourseId", "This course already exists.");
                return View(model);
            }

          
            var newCourse = new Course
            {
                CourseId = model.CourseId,
                CourseName = model.CourseName,
                PrerequestId = model.PrerequisiteId,
                CourseDepartment = model.CourseDepartment
            };

            _context.Courses.Add(newCourse);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Course added successfully!";
            return View();
        }
        public IActionResult AdminNotifications()
        {
           
            return View();
        }

        [HttpGet]
        public IActionResult AddExamPeriod()
        {
            return View(new CalendarViewModel());
        }

        [HttpPost]
        public IActionResult AddExamPeriod(CalendarViewModel model)
        {
            if (ModelState.IsValid)
            {
               
                var firstExamPeriod = _context.Calendars.FirstOrDefault(c => c.ExamType == "First");
                if (firstExamPeriod != null)
                {
                    firstExamPeriod.StartDate = model.FirstExamStartDate;
                    firstExamPeriod.EndDate = model.FirstExamEndDate;
                }

           
                var secondExamPeriod = _context.Calendars.FirstOrDefault(c => c.ExamType == "Second");
                if (secondExamPeriod != null)
                {
                    secondExamPeriod.StartDate = model.SecondExamStartDate;
                    secondExamPeriod.EndDate = model.SecondExamEndDate;
                }

                
                var midExamPeriod = _context.Calendars.FirstOrDefault(c => c.ExamType == "Mid");
                if (midExamPeriod != null)
                {
                    midExamPeriod.StartDate = model.MidExamStartDate;
                    midExamPeriod.EndDate = model.MidExamEndDate;
                }

                _context.SaveChanges();


                var coordinators = _context.Users
      .Where(u => u.FacultyRole == FacultyRole.Coordinator)
      .ToList();

                foreach (var coordinator in coordinators)
                {
                    
                    if (firstExamPeriod != null)
                    {
                        _context.Notifications.Add(new Notification
                        {
                            RecipientId = coordinator.UserId,
                            Message = $"The First Exam Period starts on {firstExamPeriod.StartDate:yyyy-MM-dd} and ends on {firstExamPeriod.EndDate:yyyy-MM-dd}. Please schedule exams accordingly.",
                            CreatedAt = DateTime.Now,
                            IsRead = false
                        });
                    }

                    if (secondExamPeriod != null)
                    {
                        _context.Notifications.Add(new Notification
                        {
                            RecipientId = coordinator.UserId,
                            Message = $"The Second Exam Period starts on {secondExamPeriod.StartDate:yyyy-MM-dd} and ends on {secondExamPeriod.EndDate:yyyy-MM-dd}. Please schedule exams accordingly.",
                            CreatedAt = DateTime.Now,
                            IsRead = false
                        });
                    }

                    if (midExamPeriod != null)
                    {
                        _context.Notifications.Add(new Notification
                        {
                            RecipientId = coordinator.UserId,
                            Message = $"The Mid Exam Period starts on {midExamPeriod.StartDate:yyyy-MM-dd} and ends on {midExamPeriod.EndDate:yyyy-MM-dd}. Please schedule exams accordingly.",
                            CreatedAt = DateTime.Now,
                            IsRead = false
                        });
                    }
                }

                _context.SaveChanges();
                TempData["SuccessMessage"] = "Exam Periods added successfully!";
                return View();
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult AddClassRoom()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult AddClassRoom(ClassRoom model)
        {
            if (ModelState.IsValid)
            {
                
                if (_context.ClassRooms.Any(cr => cr.RoomId == model.RoomId))
                {
                    ModelState.AddModelError("RoomId", "A classroom with this Room ID already exists.");
                    return View(model);
                }

           
                _context.ClassRooms.Add(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Classroom added successfully!";
                return View();
            }

            return View(model);
        }

    }
}
