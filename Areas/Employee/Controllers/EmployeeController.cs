using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models; // Include EmployeeTask model 
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace TaskManagementSystem.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Route("Employee/[controller]/[action]")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public EmployeeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult EmployeeDashboard()
        {
            int employeeId = HttpContext.Session.GetInt32("UserId") ?? 0;

            // Use EmployeeTask entity
            var tasks = _db.Tasks
                .Where(t => t.AssignedTo == employeeId)
                .OrderByDescending(t => t.AssignedDate)
                .ToList();

            ViewBag.EmployeeName = HttpContext.Session.GetString("Username");

            return View(tasks); // Model is now List<EmployeeTask>
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int taskId, string status)
        {
            var task = _db.Tasks.SingleOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                task.Status = status;
                _db.SaveChanges();
            }

            // Return OK for AJAX call
            return Ok();
        }

    }
}
