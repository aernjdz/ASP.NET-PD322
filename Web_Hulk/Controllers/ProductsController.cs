using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Web_Hulk.Models.Categories;
using Web_Hulk.Models.Products;
using Web_Hulk.Data;

using System.Diagnostics;


namespace Web_Hulk.Controllers
{
    public class ProductsController : Controller
    {
        private readonly HulkDbContext _context;
        private readonly IMapper _mapper;

        public ProductsController(HulkDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Index(ProductSearchViewModel search, string sortBy)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(search.Name))
                query = query.Where(x => x.Name.ToLower().Contains(search.Name.ToLower()));

            if (search.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == search.CategoryId.Value);

            // 
            switch (sortBy)
            {
                case "Name":
                    query = query.OrderBy(p => p.Name);
                    break;
                case "PriceAsc":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "PriceDesc":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                default:
                    query = query.OrderBy(p => p.Name);
                    break;
            }

            int count = query.Count();
            int page = search.Page ?? 1;
            int pageSize = search.PageSize;

            var products = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ProductItemViewModel>(_mapper.ConfigurationProvider)
                .ToList() ?? throw new Exception("Failed to get products");

            var categories = _context.Categories
                .ProjectTo<CategoryItemViewModel>(_mapper.ConfigurationProvider)
                .ToList() ?? throw new Exception("Failed to get categories");

            search.Categories = categories;
            //search.Page = page;
            //search.SortBy = sortBy;


            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            var model = new ProductHomeViewModel
            {
                Search = search,
                Products = products,
                Count = count,
                Pagination = new PaginationViewModel
                {
                    PageSize = pageSize,
                    TotalItems = count,
                    CurrentPage = page,
                },
                Categories = categories,
            };

            Console.WriteLine("RunTime ProductsController Index" + elapsedTime);

            return View(model);
        }

    }
}