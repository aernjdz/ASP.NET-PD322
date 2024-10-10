using Microsoft.AspNetCore.Mvc;
using StoreApi.Data;
using StoreApi.Data.Entities;
using StoreApi.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using StoreApi.Models.Product;
using Microsoft.EntityFrameworkCore;

namespace StoreApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(ApiStoreDbContext context,
        IImageTool imageTool, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetProducts()
        {
            var list = context.Products.ProjectTo<ProductItemViewModel>(mapper.ConfigurationProvider).ToList();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateViewModel model)
        {
            var entity = mapper.Map<ProductEntity>(model);
            context.Products.Add(entity);
            context.SaveChanges();

            if (model.Images != null)
            {
                var p = 1;
                foreach (var image in model.Images)
                {
                    var pi = new ProductImageEntity
                    {
                        Image = await imageTool.Save(image),
                        Priority = p,
                        ProductId = entity.Id
                    };
                    p++;
                    context.ProductImages.Add(pi);
                    await context.SaveChangesAsync();
                }
            }

            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] ProductEditViewModel model)
        {
            var product = await context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == model.Id);

            mapper.Map(model, product);

            if (model.PreviousImages != null)
            {
                foreach (var prevImage in model.PreviousImages)
                {
                    var existingImage = product.ProductImages
                        .FirstOrDefault(img => img.Id == prevImage.Id);

                    if (existingImage != null)
                        existingImage.Priority = prevImage.Priority;
                }
            }

            if (model.ImagesIds != null)
            {
                var imagesToDelete = context.ProductImages
                    .Where(img => model.ImagesIds.Contains(img.Id))
                    .ToList();

                foreach (var img in imagesToDelete)
                {
                    imageTool.Delete(img.Image);
                    context.ProductImages.Remove(img);
                }
            }

            if (model.NewImages != null)
            {
                foreach (var img in model.NewImages)
                {
                    if (img != null)
                    {
                        var imagePath = await imageTool.Save(img);
                        context.ProductImages.Add(new ProductImageEntity
                        {
                            Image = imagePath,
                            ProductId = product.Id,
                        });
                    }
                }
            }
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await context.Products
                .Include(x => x.ProductImages)
                .SingleOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = context.Products
                .Include(x => x.ProductImages)
                .SingleOrDefault(x => x.Id == id);

            if (product == null) return NotFound();

            if (product.ProductImages != null)
                foreach (var p in product.ProductImages)
                    imageTool.Delete(p.Image);

            context.Products.Remove(product);
            context.SaveChanges();
            return Ok();
        }

    }
}