using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nedi_Budi.Context;
using Nedi_Budi.Helper;
using Nedi_Budi.Models;
using Nedi_Budi.ViewModels.EmployeeViewModels;
using System.Threading.Tasks;

namespace Nedi_Budi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly string _folderPath;

        public EmployeeController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _folderPath = Path.Combine(_environment.WebRootPath, "img");
        }

        public async Task<IActionResult> Index()
        {
            var employyes = await _context.Employees.Select(x => new EmployeeGetVM()
            {
                Id=x.Id,
                Name=x.Name,
                ImagePath=x.ImagePath,
                CategoryName=x.Category.Name
            }).ToListAsync();
            return View(employyes);
        }
        public async Task<IActionResult> Create()
        {
            await _sendCategoryViewBag();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateVM vm)
        {
            await _sendCategoryViewBag();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var existCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);
            if (!existCategory)
            {
                ModelState.AddModelError("CategoryId", "Bele Categoriya yoxdur");
                return View(vm);
            }
            if (!vm.Image.CheckSize(2))
            {
                ModelState.AddModelError("Image", "Olcu maks 2mb olaa biler");
                return View(vm);
            }
            if (!vm.Image.CheckType("image"))
            {
                ModelState.AddModelError("Image", "image tipinde ol,malidi file");
                return View(vm);
            }
            string uniqueFileName = await vm.Image.FileUploadAsync(_folderPath);
            Employee employee = new()
            {
                Name=vm.Name,
                ImagePath=uniqueFileName,
                CategoryId=vm.CategoryId
            };
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if(employee is null)
            {
                return NotFound();
            }
            EmployeeUpdateVM vm = new()
            {
                Name=employee.Name,
                CategoryId=employee.CategoryId
            };
            await _sendCategoryViewBag();
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(EmployeeUpdateVM vm)
        {
            await _sendCategoryViewBag();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var existCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);
            if (!existCategory)
            {
                ModelState.AddModelError("CategoryId", "Bele Categoriya yoxdur");
                return View(vm);
            }
            if (!vm.Image?.CheckSize(2)?? false)
            {
                ModelState.AddModelError("Image", "Olcu maks 2mb olaa biler");
                return View(vm);
            }
            if (!vm.Image?.CheckType("image")??false)
            {
                ModelState.AddModelError("Image", "image tipinde ol,malidi file");
                return View(vm);
            }
            var existEmployee = await _context.Employees.FindAsync(vm.Id);
            if(existEmployee is null)
            {
                return BadRequest();
            }
            existEmployee.Name = vm.Name;
            existEmployee.CategoryId = vm.CategoryId;
            if(vm.Image is { })
            {
                string uniqueFileName = await vm.Image.FileUploadAsync(_folderPath);
                string oldFileName = Path.Combine(_folderPath, existEmployee.ImagePath);
                FileHelper.FileDelete(oldFileName);
                existEmployee.ImagePath = uniqueFileName;
            }
            _context.Employees.Update(existEmployee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if(employee is null)
            {
                return NotFound();
            }
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            string deleteFileName = Path.Combine(_folderPath, employee.ImagePath);
            FileHelper.FileDelete(deleteFileName);
            return RedirectToAction(nameof(Index));

        }
        private async Task _sendCategoryViewBag()
        {
            var category = await _context.Categories.Select(x => new SelectListItem()
            {
                Value=x.Id.ToString(),
                Text=x.Name
            }).ToListAsync();
            ViewBag.Categories = category;
        }
    }
}
