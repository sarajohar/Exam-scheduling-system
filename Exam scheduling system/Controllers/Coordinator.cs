using Microsoft.AspNetCore.Mvc;
using ExamSchedulingSystem.Data;
using ExamSchedulingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc.Rendering;
namespace ExamSchedulingSystem.Controllers
{
    public class Coordinator : Controller
    {
        private readonly ApplicationDbContext _context;

        public Coordinator(ApplicationDbContext context)
        {
            _context = context;
        }
        

        [HttpGet]
        public IActionResult ReserveAnExam()
        {
            var coordinatorId = HttpContext.Session.GetString("CoordinatorId");


            ViewBag.Courses = _context.Lectures
                .Where(l => l.UserId == coordinatorId && l.UserRole == "Coordinater")
                .Select(l => l.Course)
                .Distinct()
                .ToList();

            return View(new ReserveExamViewModel());
        }

        [HttpGet]
        public JsonResult GetExamPeriod(string examType)
        {
            var period = _context.Calendars.FirstOrDefault(c => c.ExamType == examType);
            if (period != null)
            {
                return Json(new
                {
                    StartDate = period.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = period.EndDate.ToString("yyyy-MM-dd")
                });
            }
            return Json(null);
        }



        [HttpGet]
        public JsonResult GetAvailableTimeSlots(DateTime selectedDate, int courseId)
        {
            var day = selectedDate.DayOfWeek.ToString();
            string courseIdStr = courseId.ToString();
            char courseYearLevel = courseIdStr.Length >= 8 ? courseIdStr[7] : '\0';


            bool isYearLevelConflict = _context.ExamReservations
         .Where(r => r.ExamDate == selectedDate && r.StartTime != null && r.CourseId != courseId)
         .AsEnumerable()
         .Any(r => r.CourseId.ToString().Length >= 8 && r.CourseId.ToString()[7] == courseYearLevel);


            if (isYearLevelConflict)
            {
                return Json(new List<object>());
            }


            var lectureSlots = _context.Lectures
                .Where(l => l.TimeSlot != null && l.TimeSlot.Day == day)
                .Select(l => l.TimeSlot.StartTime)
                .Distinct()
                .ToList();



            var examReservedRooms = _context.ExamReservations
                 .Where(r => r.ExamDate == selectedDate && r.StartTime != null && r.RoomId != null)
                 .Select(r => new { r.StartTime, r.RoomId })
                 .ToList();


            var lectureReservedRooms = _context.Lectures
                .Where(l => l.TimeSlot != null && l.TimeSlot.Day == day && l.RoomId != null && l.TimeSlot.StartTime != null)
                .Select(l => new { StartTime = l.TimeSlot.StartTime, RoomId = l.RoomId })
                .ToList();


            var reservedRoomsByTimeSlot = examReservedRooms
                .Concat(lectureReservedRooms)
                .GroupBy(r => r.StartTime)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(r => r.RoomId).Distinct().Count()
                );


            var totalRoomsCount = _context.ClassRooms.Count();

            // Filter out time slots where all rooms are occupied
            var availableSlots = _context.TimeSlots
                .Where(ts => ts.Day == day)
                .AsEnumerable()
                .Where(ts =>
                    ts.StartTime != null && ts.EndTime != null &&
                    (!reservedRoomsByTimeSlot.ContainsKey(ts.StartTime) ||     // Slot is available if no rooms are reserved
                     reservedRoomsByTimeSlot[ts.StartTime] < totalRoomsCount)  // Or if not all rooms are reserved
                )
                .Select(ts => new
                {
                    ts.SlotId,
                    startTime = ts.StartTime.ToString(@"hh\:mm\:ss"),
                    endTime = ts.EndTime.ToString(@"hh\:mm\:ss"),
                    timeRange = $"{ts.StartTime:hh\\:mm} - {ts.EndTime:hh\\:mm}"
                })
                .ToList();

            return Json(availableSlots);
        }

        [HttpGet]
        public JsonResult GetAvailableRooms(DateTime selectedDate, TimeSpan startTime, TimeSpan endTime)
        {

            var reservedRoomsForExams = _context.ExamReservations
                .Where(r => r.ExamDate == selectedDate && r.StartTime == startTime && r.EndTime == endTime)
                .Select(r => r.RoomId)
                .ToList();


            var lectureRooms = _context.Lectures
                .Where(l => l.TimeSlot.StartTime == startTime && l.TimeSlot.EndTime == endTime)
                .Select(l => l.RoomId)
                .Distinct()
                .ToList();


            var availableRooms = _context.ClassRooms
                .Where(cr => !reservedRoomsForExams.Contains(cr.RoomId) && !lectureRooms.Contains(cr.RoomId))
                .Select(cr => new
                {
                    cr.RoomId,
                    cr.Capacity
                })
                .ToList();

            return Json(availableRooms);
        }



        [HttpPost]
        public IActionResult ReserveAnExam(ReserveExamViewModel model)
        {
            try
            {
                Console.WriteLine("ReserveAnExam action started.");
                Console.WriteLine($"Received SelectedTimeSlot: {model.SelectedTimeSlot}");

                if (!ModelState.IsValid)
                {
                    Console.WriteLine("ModelState is invalid.");
                    ViewBag.Courses = _context.Courses.ToList();
                    return View(model);
                }

                if (!string.IsNullOrEmpty(model.SelectedTimeSlot))
                {
                    var timeParts = model.SelectedTimeSlot.Split('-');
                    if (timeParts.Length == 2 &&
                        TimeSpan.TryParse(timeParts[0].Trim(), out var startTime) &&
                        TimeSpan.TryParse(timeParts[1].Trim(), out var endTime))
                    {
                        Console.WriteLine($"Parsed StartTime: {startTime}, EndTime: {endTime}");


                        var existingSchedule = _context.ExamSchedules
                            .FirstOrDefault(es =>
                                es.CourseId == model.SelectedCourseId &&
                                es.ExamDate == model.SelectedDate &&
                                es.StartTime == startTime &&
                                es.EndTime == endTime);

                        if (existingSchedule != null)
                        {

                            existingSchedule.Place += $", {model.SelectedRoomId}";
                        }
                        else
                        {

                            var examSchedule = new ExamSchedule
                            {
                                CourseId = model.SelectedCourseId,
                                CourseName = _context.Courses
                                    .Where(c => c.CourseId == model.SelectedCourseId)
                                    .Select(c => c.CourseName)
                                    .FirstOrDefault(),
                                ExamDate = model.SelectedDate ?? DateTime.Today,
                                StartTime = startTime,
                                EndTime = endTime,
                                Place = model.SelectedRoomId
                            };
                            _context.ExamSchedules.Add(examSchedule);
                        }


                        var reservation = new ExamReservation
                        {
                            CourseId = model.SelectedCourseId,
                            CourseName = _context.Courses
                                .Where(c => c.CourseId == model.SelectedCourseId)
                                .Select(c => c.CourseName)
                                .FirstOrDefault(),
                            ExamDate = model.SelectedDate ?? DateTime.Today,
                            StartTime = startTime,
                            EndTime = endTime,
                            RoomId = model.SelectedRoomId,
                            CoordinatorId = HttpContext.Session.GetString("CoordinatorId") ?? "TestCoordinator",
                            ExamType = model.SelectedExamType ?? ExamType.First,
                            InvigilatorName = model.SelectedInvigilatorName ?? "None"
                        };

                        _context.ExamReservations.Add(reservation);


                        var students = _context.Users
                            .Where(u => u.Role == UserRole.Student)
                            .Select(u => u.UserId)
                            .ToList();

                        foreach (var studentId in students)
                        {
                            var notification = new Notification
                            {
                                RecipientId = studentId,
                                Message = $"Exam for course {reservation.CourseName} has been scheduled on {reservation.ExamDate:yyyy-MM-dd}" +
                                $" from {reservation.StartTime:hh\\:mm}" +
                                $" to {reservation.EndTime:hh\\:mm} in room {reservation.RoomId}."
                            };
                            _context.Notifications.Add(notification);
                        }

                        _context.SaveChanges();
                        Console.WriteLine("Reservation saved successfully.");
                        return RedirectToAction("CoordinatorDashboard", "Home");
                    }
                    else
                    {
                        Console.WriteLine("Invalid time slot format.");
                    }
                }
                else
                {
                    Console.WriteLine("SelectedTimeSlot is empty or null.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return Content("Error: " + ex.Message);
            }

            ViewBag.Courses = _context.Courses.ToList();
            return View(model);
        }


        [HttpGet]
        public IActionResult ChooseInvigilator()
        {
            //  reservations made by the coordinator without an assigned invigilator
            string coordinatorId = HttpContext.Session.GetString("CoordinatorId");
            var reservations = _context.ExamReservations
                .Where(r => r.CoordinatorId == coordinatorId && r.InvigilatorName == "None")
                .ToList();

            ViewBag.Reservations = reservations;

            return View();
        }

        [HttpGet]
        public JsonResult GetAvailableInvigilators(int reservationId)
        {
            var coordinatorId = HttpContext.Session.GetString("CoordinatorId");

            var reservation = _context.ExamReservations
                .FirstOrDefault(r => r.ReservationId == reservationId && r.CoordinatorId == coordinatorId);
            if (reservation == null) return Json(new List<string>());

            //  conflicting invigilators based on overlapping exam times
            var conflictingInvigilators = _context.ExamReservations
                .Where(r => r.ExamDate == reservation.ExamDate &&
                            r.StartTime < reservation.EndTime &&
                            r.EndTime > reservation.StartTime &&
                            !string.IsNullOrEmpty(r.InvigilatorName))
                .Select(r => r.InvigilatorName)
                .ToList();

            //  available invigilators to only faculty with the 'Invigilator' role
            var availableInvigilators = _context.Users
                .Where(u => u.Role == UserRole.Faculty &&
                            u.FacultyRole == FacultyRole.Invigilator &&
                            !conflictingInvigilators.Contains(u.Name))
                .Select(u => u.Name)
                .ToList();

            return Json(availableInvigilators);
        }


        [HttpPost]
        public IActionResult ChooseInvigilator(int reservationId, string invigilatorName)
        {
            var coordinatorId = HttpContext.Session.GetString("CoordinatorId");

            var reservation = _context.ExamReservations
                .FirstOrDefault(r => r.ReservationId == reservationId && r.CoordinatorId == coordinatorId);
            if (reservation != null)
            {
                reservation.InvigilatorName = invigilatorName;
                _context.SaveChanges();
                return Json(new { success = true, message = "Invigilator assigned successfully." });
            }
            return Json(new { success = false, message = "Reservation not found." });
        }


        [HttpGet]
        public IActionResult ManageReservation()
        {
            var coordinatorId = HttpContext.Session.GetString("CoordinatorId");
            var reservations = _context.ExamReservations
                .Where(r => r.CoordinatorId == coordinatorId)
                .ToList();


            return View(reservations);
        }

        [HttpGet]
        public IActionResult EditReservation(int reservationId)
        {
            var coordinatorId = HttpContext.Session.GetString("CoordinatorId");


            var reservation = _context.ExamReservations.Find(reservationId);
            if (reservation == null)
            {
                return NotFound("Reservation not found.");
            }


            ViewBag.Courses = _context.Lectures
                .Where(l => l.UserId == coordinatorId && l.UserRole == "Coordinater")
                .Select(l => l.Course)
                .Distinct()
                .ToList();


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
                    var coordinatorId = HttpContext.Session.GetString("CoordinatorId");


                    ViewBag.Courses = _context.Lectures
                        .Where(l => l.UserId == coordinatorId && l.UserRole == "Coordinater")
                        .Select(l => l.Course)
                        .Distinct()
                        .ToList();

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


                reservation.CourseId = model.SelectedCourseId;
                reservation.CourseName = _context.Courses
                    .Where(c => c.CourseId == model.SelectedCourseId)
                    .Select(c => c.CourseName)
                    .FirstOrDefault();
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
                reservation.ExamType = model.SelectedExamType ?? ExamType.First;


                existingSchedule.CourseId = reservation.CourseId;
                existingSchedule.CourseName = reservation.CourseName;
                existingSchedule.ExamDate = reservation.ExamDate;
                existingSchedule.StartTime = reservation.StartTime;
                existingSchedule.EndTime = reservation.EndTime;

                existingSchedule.Place = reservation.RoomId;

                _context.SaveChanges();

                return RedirectToAction("ManageReservation");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the reservation.");
                return View(model);
            }
        }



        [HttpGet]
        public IActionResult DeleteReservation(int id)
        {
            var coordinatorId = HttpContext.Session.GetString("CoordinatorId");


            var reservation = _context.ExamReservations
                .FirstOrDefault(r => r.ReservationId == id && r.CoordinatorId == coordinatorId);

            if (reservation == null)
            {
                return NotFound();
            }


            var examSchedule = _context.ExamSchedules
                .FirstOrDefault(es => es.CourseId == reservation.CourseId
                    && es.ExamDate == reservation.ExamDate
                    && es.StartTime == reservation.StartTime
                    && es.EndTime == reservation.EndTime);


            if (examSchedule != null)
            {
                _context.ExamSchedules.Remove(examSchedule);
            }


            _context.ExamReservations.Remove(reservation);


            _context.SaveChanges();

            return RedirectToAction("ManageReservation");
        }


        [HttpGet]
        public IActionResult EditInvigilator(int id)
        {
            var coordinatorId = HttpContext.Session.GetString("CoordinatorId");
            var reservation = _context.ExamReservations.FirstOrDefault(r => r.ReservationId == id && r.CoordinatorId == coordinatorId);

            if (reservation == null)
            {
                return NotFound();
            }


            var conflictingInvigilators = _context.ExamReservations
                .Where(r => r.ExamDate == reservation.ExamDate &&
                            r.StartTime < reservation.EndTime &&
                            r.EndTime > reservation.StartTime &&
                            !string.IsNullOrEmpty(r.InvigilatorName))
                .Select(r => r.InvigilatorName)
                .ToList();

            var availableInvigilators = _context.Users
                .Where(u => u.Role == UserRole.Faculty &&
                            u.FacultyRole == FacultyRole.Invigilator &&
                            !conflictingInvigilators.Contains(u.Name))
                .Select(u => u.Name)
                .ToList();

            var model = new EditInvigilatorViewModel
            {
                ReservationId = reservation.ReservationId,
                CurrentInvigilator = reservation.InvigilatorName,
                AvailableInvigilators = availableInvigilators
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult EditInvigilator(EditInvigilatorViewModel model)
        {
            var coordinatorId = HttpContext.Session.GetString("CoordinatorId");
            var reservation = _context.ExamReservations.FirstOrDefault(r => r.ReservationId == model.ReservationId && r.CoordinatorId == coordinatorId);

            if (reservation == null)
            {
                return NotFound();
            }

            reservation.InvigilatorName = model.NewInvigilator;
            _context.SaveChanges();

            return RedirectToAction("ManageReservation");
        }


        [HttpGet]
        public IActionResult ViewExcusesList()
        {
            var coordinatorId = HttpContext.Session.GetString("CoordinatorId");
            var excuses = _context.Excuses
                .Where(e => e.Reservation.CoordinatorId == coordinatorId)
                .Select(e => new ExcuseViewModel
                {
                    ExcuseId = e.ExcuseId,
                    ExamName = e.Reservation.CourseName,
                    InvigilatorName = e.InvigilatorName,
                    ExcuseText = e.ExcuseText,
                    ExamDate = e.Reservation.ExamDate,
                    StartTime = e.Reservation.StartTime,
                    EndTime = e.Reservation.EndTime,
                    IsAccepted = e.IsAccepted,
                    Status = e.IsAccepted.HasValue ? (e.IsAccepted.Value ? "Accepted" : "Rejected") : "Pending"
                })
                .ToList();

            return View(excuses);
        }

        [HttpPost]
        public IActionResult RespondToExcuse(int excuseId, bool isAccepted)
        {
            var excuse = _context.Excuses.Find(excuseId);
            if (excuse != null)
            {
                excuse.IsAccepted = isAccepted;
                _context.SaveChanges();
            }
            return RedirectToAction("ViewExcusesList");
        }

        [HttpGet]
        public IActionResult ViewChangeRequests()
        {
            var coordinatorName = HttpContext.Session.GetString("UserName");
            var requests = _context.ChangeRequests
                .Where(cr => cr.CoordinatorName == coordinatorName)
                .Select(cr => new ChangeRequestViewModel
                {
                    RequestId = cr.RequestId,
                    TeacherName = cr.TeacherName,
                    ExamName = cr.Reservation.CourseName,
                    RequestText = cr.RequestText,
                    ExamDate = cr.Reservation.ExamDate,
                    StartTime = cr.Reservation.StartTime,
                    EndTime = cr.Reservation.EndTime,
                    IsAccepted = cr.IsAccepted,
                    Status = cr.IsAccepted.HasValue ? (cr.IsAccepted.Value ? "Accepted" : "Rejected") : "Pending"



                })
                .ToList();
            return View(requests);
        }

        [HttpPost]
        public IActionResult RespondToChangeRequest(int requestId, bool response)
        {
            var request = _context.ChangeRequests.Find(requestId);
            if (request != null)
            {
                request.IsAccepted = response;
                _context.SaveChanges();
            }
            return RedirectToAction("ViewChangeRequests");
        }

        public IActionResult CoordinatorNotifications()
        {
            var coordinatorId = HttpContext.Session.GetString("CoordinatorId");
            if (coordinatorId == null)
            {
                return RedirectToAction("Login", "Home");
            }


            var notifications = _context.Notifications
                .Where(n => n.RecipientId == coordinatorId)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();

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
            return RedirectToAction("CoordinatorNotifications");

        }
    }
}