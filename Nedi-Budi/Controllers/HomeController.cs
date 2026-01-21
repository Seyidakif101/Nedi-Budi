using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nedi_Budi.Context;
using Nedi_Budi.ViewModels.EmployeeViewModels;
using System.Diagnostics;

namespace Nedi_Budi.Controllers
{
    public class HomeController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var employyes = await _context.Employees.Select(x => new EmployeeGetVM()
            {
                Id = x.Id,
                Name = x.Name,
                ImagePath = x.ImagePath,
                CategoryName = x.Category.Name
            }).ToListAsync();
            return View(employyes);
        }

    }
}
