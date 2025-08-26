using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TaskManagementSystem.Models;
using TaskManagementSystem.Data;

namespace TaskManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public AccountController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_dbContext.Users.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError("", "Username already taken.");
                return View(model);
            }

            var passwordHasher = new PasswordHasher<object>();
            string hashedPassword = passwordHasher.HashPassword(null, model.Password);

            var user = new User
            {
                FullName = model.FullName,
                Phone = model.Phone,
                Username = model.Username,
                PasswordHash = hashedPassword,
                Role = "Employee" 
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _dbContext.Users.SingleOrDefault(u => u.Username == model.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            var passwordHasher = new PasswordHasher<object>();
            var result = passwordHasher.VerifyHashedPassword(null, user.PasswordHash, model.Password);

            if (result == PasswordVerificationResult.Success)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                string controllerName = user.Role;                 // "Employee" or "Manager"
                string actionName = user.Role + "Dashboard";       // "EmployeeDashboard" or "ManagerDashboard"

                return RedirectToAction(actionName, controllerName, new { area = user.Role });
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }
    }
}
