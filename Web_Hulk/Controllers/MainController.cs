using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Web_Hulk.Data;
using Web_Hulk.Data.Entities;
using Web_Hulk.Models.Categories;

namespace Web_Hulk.Controllers
{
    public class MainController : Controller
    {
        private readonly HulkDbContext _context;
        public MainController( HulkDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var list = _context.Categories.Select(x => new CategoryItemViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Image = x.Image
            }).ToList();
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
        return View(); 
        }
    
        [HttpPost]
        public IActionResult Create(CategoryCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var categoryEntity = new CategoryEntity
                {
                    Name = model.Name,
                    Image = model.Image
                };
                _context.Categories.Add(categoryEntity);
                _context.SaveChanges();
                ViewBag.Message = "Category created successfully!";
                Console.WriteLine("Category created successfully!");
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
