using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Data;
using StoreApi.Data.Entities;
using StoreApi.Interfaces;
using AutoMapper;
using StoreApi.Models.Category;
using Microsoft.EntityFrameworkCore;
namespace StoreApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ApiStoreDbContext context,
    IImageTool imageTool, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetList()
        {
            var list = context.Categories.ProjectTo<CategoryItemViewModel>(mapper.ConfigurationProvider)
                .ToList();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryCreateViewModel model)
        {
            var imageName = await imageTool.Save(model.Image);
            var entity = mapper.Map<CategoryEntity>(model);
            entity.Image = imageName;
            context.Categories.Add(entity);
            context.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] CategoryEditViewModel model)
        {
            if (model == null) return NotFound();
            var category = context.Categories.SingleOrDefault(x => x.Id == model.Id);
            category = mapper.Map(model, category);
            if (model.Image != null)
            {
                imageTool.Delete(category.Image);
                string fname = await imageTool.Save(model.Image);
                category.Image = fname;
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var list = context.Categories
                .ProjectTo<CategoryItemViewModel>(mapper.ConfigurationProvider)
                .SingleOrDefault(x => x.Id == id);
            return Ok(list);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ctgr = context.Categories.SingleOrDefault(x => x.Id == id);
            if (ctgr == null) return NotFound();

            if (!string.IsNullOrEmpty(ctgr.Image))
                imageTool.Delete(ctgr.Image);

            context.Categories.Remove(ctgr);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet("names")]
        public async Task<IActionResult> GetCategoriesNames()
        {
            var result = await context.Categories
            .ProjectTo<SelectItemViewModel>(mapper.ConfigurationProvider).ToListAsync();
            return Ok(result);
        }

    }
}