using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Nedi_Budi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
