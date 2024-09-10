using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Web_Hulk.Models.Categories;
using Web_Hulk.Data;
using Web_Hulk.Data.Entities;

namespace Web_Hulk.Controllers
{
    public class MainController : Controller
    {
        private readonly HulkDbContext context;
        private readonly IMapper _mapper;

        public MainController(HulkDbContext context, IMapper mapper)
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
    }
}