using System.Diagnostics;
using HotelManagment.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagment.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 404)
            {
                ViewBag.ErrorMessage = "La pagina che stai cercando non esiste.";
            }
            else if (statusCode == 500)
            {
                ViewBag.ErrorMessage = "Si � verificato un errore interno del server.";
            }
            else
            {
                ViewBag.ErrorMessage = "Si � verificato un errore.";
            }

            return View();
        }
    }
}
