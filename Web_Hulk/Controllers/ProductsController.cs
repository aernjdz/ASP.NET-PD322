using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web_Hulk.Data;
using Web_Hulk.Data.Entities;
using Web_Hulk.Models.Products;

namespace Web_Hulk.Controllers
{
    public class ProductsController : Controller

    {
        public readonly HulkDbContext _context;
        private readonly IMapper _mapper;

        public ProductsController(HulkDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ActionResult Index() 
        {
            var list = _context.Products.ProjectTo<ProductItemViewModel>(_mapper.ConfigurationProvider).ToList() ?? throw new Exception("Failed to get products");

            return View(list);

        }

        [HttpGet]

        public ActionResult Create() 
        {
            var categories = _context.Categories
              .Select(x => new { Value = x.Id, Text = x.Name })
              .ToList();

            ProductCreateViewModel viewModel = new()
            {
                CategoryList = new SelectList(categories, "Value", "Text")
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var prod = new Product
            {
                Name = model.Name,
                Price = model.Price,
                CategoryId = model.CategoryId,
            };

            await _context.Products.AddAsync(prod);
            await _context.SaveChangesAsync();

            if (model.Photos != null)
            {
                int i = 0;
                foreach (var img in model.Photos)
                {
                    string ext = Path.GetExtension(img.FileName);

                    string fName = Guid.NewGuid().ToString() + ext;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "images", fName);

                    using (var fs = new FileStream(path, FileMode.Create))
                        await img.CopyToAsync(fs);

                    var imgEntity = new ProductImage
                    {
                        Image = fName,
                        Priority = i++,
                        Product = prod,
                    };
                    _context.ProductImages.Add(imgEntity);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _context.Products
                .ProjectTo<ProductEditViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault(x => x.Id == id)
                ?? throw new Exception("An error occurred while receiving the product");

            var categories = _context.Categories
                .Select(x => new { Value = x.Id, Text = x.Name })
                .ToList();

            model.CategoryList = new SelectList(categories, "Value", "Text");

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == model.Id)
                ?? throw new Exception("No product was found");

            _mapper.Map(model, product);

            if (model.NewImages != null)
            {
                foreach (var img in model.NewImages)
                {
                    if (img.Length > 0)
                    {
                        string ext = Path.GetExtension(img.FileName);
                        string fName = Guid.NewGuid().ToString() + ext;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "images", fName);

                        using (var fs = new FileStream(path, FileMode.Create))
                            await img.CopyToAsync(fs);

                        var imgEntity = new ProductImage
                        {
                            Image = fName,
                            Product = product
                        };
                        _context.ProductImages.Add(imgEntity);
                    }
                }
            }

            if (model.DeletedPhotoIds != null)
            {
                var photos = _context.ProductImages
                    .Where(pi => model.DeletedPhotoIds.Contains(pi.Id))
                    .ToList();

                _context.ProductImages.RemoveRange(photos);

                foreach (var photo in photos)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "images", photo.Image);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                }
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .SingleOrDefaultAsync(p => p.Id == id) ?? throw new InvalidDataException("Product not found");

            foreach (var img in product.ProductImages)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "images", img.Image);

                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }

            _context.ProductImages.RemoveRange(product.ProductImages);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
