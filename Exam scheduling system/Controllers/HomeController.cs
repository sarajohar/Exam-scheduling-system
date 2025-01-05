using Exam_scheduling_system.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Exam_scheduling_system.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Default landing page action
        public IActionResult Index()
        {
            return View();
        }

        // Action for Faculty
        public IActionResult FacultyLogin()
        {
            return View();
        }

        // Action for Student
        public IActionResult StudentLogin()
        {
            return View();
        }

        // Action for Admin
        public IActionResult AdminLogin()
        {
            return View();
        }

        // Action for Guest
        public IActionResult Guest()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        [Route("forgot-password")]
        public IActionResult RequestPasswordResetGet(string role)
        {
            TempData["UserRole"] = role;
            return View("RequestPasswordReset");
        }

        [HttpPost]
        [Route("forgot-password/send")]
        public IActionResult RequestPasswordReset(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                return RedirectToAction("VerifyIdentity");
            }
            else
            {
                ModelState.AddModelError("", "Please enter a valid email address.");
                return View();
            }
        }

        public string GenerateResetCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // Generates a 6-digit code
        }

        public IActionResult VerifyIdentity()
        {
            // Display the verification page where users enter the code
            return View();
        }

        [HttpPost]
        public IActionResult VerifyIdentity(string verificationCode)
        {
            // Logic to verify the entered code
            if (verificationCode == "123456") // Example validation
            {
                return RedirectToAction("ResetPassword"); // Redirect to password reset page
            }
            else
            {
                // If invalid code, show an error message
                ModelState.AddModelError("", "Invalid verification code. Please try again.");
                return View();
            }
        }
     
        
            [HttpGet]
            public IActionResult ResetPassword()
            {
                // Display the reset password form
                return View();
            }

            [HttpPost]
        public IActionResult ResetPassword(string newPassword, string confirmPassword)
        {
            if (newPassword == confirmPassword)
            {
                // Retrieve the role to determine where to redirect the user after resetting the password
                string?userRole = TempData["UserRole"]?.ToString();

                TempData["SuccessMessage"] = "Your password has been successfully reset.";

                // Redirect to the respective login page based on the user's role
                switch (userRole)
                {
                    case "Student":
                        return RedirectToAction("StudentLogin");
                    case "Faculty":
                        return RedirectToAction("FacultyLogin");
                    case "Admin":
                        return RedirectToAction("AdminLogin");
                    default:
                        return RedirectToAction("Index"); // Default to home page if no role is found
                }
            }
            else
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return View();
            }
        }

        public IActionResult SignUp(string role)
        {
            // Store the role so we can use it throughout the process
            TempData["UserRole"] = role; // Store the role temporarily
            return View();
        }

        // POST: Handle form submission for Sign Up
        [HttpPost]
        public IActionResult SignUp(string name, string email, string id, string password, string confirmPassword)
        {
            // Basic validation: Check if passwords match
            if (password == confirmPassword)
            {
                // Simulate saving the user's information (you would save it to the database here)
                // Redirect to the login page based on the role
                string? userRole = TempData["UserRole"]?.ToString();

                // Redirect based on role
                switch (userRole)
                {
                    case "Student":
                        return RedirectToAction("StudentLogin");
                    case "Faculty":
                        return RedirectToAction("FacultyLogin");
                    case "Admin":
                        return RedirectToAction("AdminLogin");
                    default:
                        return RedirectToAction("Index"); // Default to home page if no role is found
                }
            }
            else
            {
                // If passwords don't match, return an error and re-render the form
                ModelState.AddModelError("", "Passwords do not match.");
                return View();
            }
        }








        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
