using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_Hulk.Constants;
using Web_Hulk.Data.Entities;
using Web_Hulk.Data;
using AutoMapper.QueryableExtensions;

using Web_Hulk.Areas.Admin.Models.Categories;

namespace Web_Hulk.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class CategoriesController : Controller
    {
        private readonly HulkDbContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(HulkDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ProjectTo<CategoryItemViewModel>(_mapper.ConfigurationProvider).ToList() ?? throw new Exception("Failed to get products");


            if (categories == null) Console.WriteLine("Categories is null..");
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CategoryCreateViewModel createModel)
        {
            if (!ModelState.IsValid)
                return View(createModel);

            string ext = Path.GetExtension(createModel.Image.FileName);
            string fName = Guid.NewGuid().ToString() + ext;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "images", fName);

            using (var stream = new FileStream(path, FileMode.Create))
                await createModel.Image.CopyToAsync(stream);

            _context.Categories.Add(new CategoryEntity
            {
                Name = createModel.Name,
                Image = fName
            });
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _context.Categories
                .Where(c => c.Id == id)
                .ProjectTo<CategoryEditViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault() ?? throw new InvalidDataException($"Item with such id={id} doesn`t exist");

            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(CategoryEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var item = _context.Categories.Find(model.Id) ?? throw new InvalidDataException("Category not found");

            item.Name = model.Name;
            var images = Path.Combine(Directory.GetCurrentDirectory(), "images");

            if (model.NewImage != null)
            {
                var currentImg = Path.Combine(images, item.Image);
                if (System.IO.File.Exists(currentImg))
                    System.IO.File.Delete(currentImg);

                var newImg = Guid.NewGuid().ToString() + Path.GetExtension(model.NewImage.FileName);
                var newPath = Path.Combine(images, newImg);
                using (var stream = new FileStream(newPath, FileMode.Create))
                    model.NewImage.CopyTo(stream);

                item.Image = newImg;
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _context.Categories.Find(id) ?? throw new InvalidDataException("Category not found");

            var path = Path.Combine(Directory.GetCurrentDirectory(), "images", item.Image);
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);

            _context.Categories.Remove(item);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
