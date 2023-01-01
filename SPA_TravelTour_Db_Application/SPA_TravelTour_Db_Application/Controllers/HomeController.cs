using Microsoft.AspNetCore.Mvc;

namespace SPA_TravelTour_Db_Application.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
