 using Microsoft.AspNetCore.Mvc;

namespace TaskManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Message = "Welcome to Task Management System!";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}