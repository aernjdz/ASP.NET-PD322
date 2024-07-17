using Microsoft.AspNetCore.Mvc;
using Web_Hulk.Data;
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
    }
}
