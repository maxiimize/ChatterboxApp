using System.Diagnostics;
using ChatterboxApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatterboxApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
