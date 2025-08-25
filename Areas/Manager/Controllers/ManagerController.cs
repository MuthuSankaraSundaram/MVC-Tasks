using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace TaskManagementSystem.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Route("Manager/[controller]/[action]")]
    public class ManagerController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ManagerController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Add Task
        [HttpGet]
        public IActionResult AddTask()
        {
            PopulateEmployees();
            return View(new EmployeeTask());
        }

        // POST: Add Task
        [HttpPost]
        public IActionResult AddTask(EmployeeTask model)
        {
            if (ModelState.IsValid)
            {
                model.AssignedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
                model.AssignedDate = DateTime.Now;
                model.Status = "Pending";

                _dbContext.Tasks.Add(model);
                _dbContext.SaveChanges();

                return RedirectToAction("ManagerDashboard", "Manager", new { area = "Manager" });

            }

            // reload employees if model invalid
            ViewBag.Employees = _dbContext.Users
                .Where(u => u.Role == "Employee")
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Username
                }).ToList();

            return View(model);
        }


        // Manager Dashboard
        public IActionResult ManagerDashboard()
        {
            int managerId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var tasks = (from t in _dbContext.Tasks
                         join e in _dbContext.Users on t.AssignedTo equals e.Id
                         where t.AssignedBy == managerId
                         select new
                         {
                             t.Title,
                             t.Description,
                             EmployeeName = e.Username,
                             t.Status,
                             t.AssignedDate
                         }).ToList();

            return View(tasks);
        }


        private void PopulateEmployees()
        {
            ViewBag.Employees = _dbContext.Users
                .Where(u => u.Role == "Employee")
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Username
                }).ToList();
        }
    }
}
