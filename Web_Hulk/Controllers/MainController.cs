using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Web_Hulk.Data;
using Web_Hulk.Data.Entities;
using Web_Hulk.Models.Categories;

namespace Web_Hulk.Controllers
{
    public class MainController : Controller
    {
        private readonly HulkDbContext context;
        private IMapper _mapper;
        public MainController( HulkDbContext context, IMapper mapper)
        {
            this.context = context;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var categories = context.Categories.ProjectTo<CategoryItemViewModel>(_mapper.ConfigurationProvider).ToList();

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
            if(!ModelState.IsValid) return View(createModel);

            string ext = Path.GetExtension(createModel.Image.FileName);
            string fName = Guid.NewGuid().ToString() + ext;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "images", fName);

            using (var stream = new FileStream(path,FileMode.Create))
                await createModel.Image.CopyToAsync(stream);

            context.Categories.Add(new CategoryEntity
            {
                Name = createModel.Name,
                Image = fName
            });

            context.SaveChanges();

            return RedirectToAction(nameof(Index));

        }
        [HttpGet]

        public IActionResult Edit(int id)
        {
            var item = context.Categories.Where(c => c.Id == id).ProjectTo<CategoryEditViewModel>(_mapper.ConfigurationProvider).FirstOrDefault() ?? throw new InvalidDataException($"Item with such id={id} doesn`t exist");

            return View(item);

        }

        [HttpPost]
        public IActionResult Edit(CategoryEditViewModel model)
        {
            if(!ModelState.IsValid) return View(model);

            var item = context.Categories.Find(model.Id) ?? throw new InvalidDataException("Category not found");

            item.Name = model.Name;
           
            var images = Path.Combine(Directory.GetCurrentDirectory(), "images");

            if(model.NewImage != null)
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

            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

       [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = context.Categories.Find(id) ?? throw new InvalidDataException("Category not found");

            var path = Path.Combine(Directory.GetCurrentDirectory(), "images", item.Image);
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);

            context.Categories.Remove(item);
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
