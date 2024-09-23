using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web_Hulk.Constants;
using Web_Hulk.Data.Entities.Identity;
using Web_Hulk.Data.Entities;
using Web_Hulk.Data;
using Bogus;
using System.Net;
using Web_Hulk.interfaces;
namespace Web_Hulk.Services
{
    public class DataSeeder { 
  private readonly HulkDbContext _context;
    private readonly UserManager<UserEntity> _userManager;
    private readonly RoleManager<RoleEntity> _roleManager;
    private readonly IImageWorker _imageWorker;

    public DataSeeder(HulkDbContext context, UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager, IImageWorker imageWorker)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
            _imageWorker = imageWorker;
    }
        public async Task SeedProducts()
        {
            if (!_context.Products.Any())
            {
                var fakerCateory = new Faker<CategoryEntity>(locale: "pl")
                 .RuleFor(u => u.Name, (f, u) => f.Commerce.Categories(1)[0]);

                var category = fakerCateory.Generate(10);

                string url = "https://picsum.photos/1200/800?category";

                category.ForEach(t =>
                {

                    var img = _imageWorker.ImageSave(url);
                    if (!String.IsNullOrEmpty(img))
                    {
                        t.Image = img;
                    }
                    _context.Categories.Add(t);
                });

                _context.SaveChanges();

                var categories = _context.Categories.ToList();

                  var fakerProduct = new Faker<Product>("pl")
                      .RuleFor(u => u.Name, (f, u) => f.Commerce.Product())
                      .RuleFor(u => u.Price, (f, u) => decimal.Parse(f.Commerce.Price()))
                    .RuleFor(u=> u.Category, (f, u) => f.PickRandom(categories));

                string url_ = "https://picsum.photos/1200/800?product";

                var products = fakerProduct.GenerateLazy(1000);
                Random r = new Random();

                foreach (var product in products)
                {
                    _context.Add(product);
                    _context.SaveChanges();
                    int imageCount = r.Next(3, 5);
                    for (int i = 0; i < imageCount; i++)
                    {
                        var imageName = _imageWorker.ImageSave(url);
                        var imageProduct = new ProductImage
                        {
                            Product = product,
                            Image = imageName,
                            Priority = i
                        };
                        _context.Add(imageProduct);
                        _context.SaveChanges();
                    }

                }
            }
        }




        public async Task SeedRolesAndUsers()
    {
        // seed roles
        if (_context.Roles.Count() == 0)
        {
            var roles = new[]
            {
                new RoleEntity { Name = Roles.Admin },
                    new RoleEntity { Name = Roles.User }
                };

            foreach (var role in roles)
            {
                var outcome = await _roleManager.CreateAsync(role);
                if (!outcome.Succeeded) Console.WriteLine($"Failed to create role: {role.Name}");
            }
        }

        // seed users
        if (_context.Users.Count() == 0)
        {
            var users = new[]
           {
                    new { User = new UserEntity { FirstName = "Tony", LastName = "Stark", UserName = "admin1", Email = "admin@gmail.com" }, Password = "admin1", Role = Roles.Admin },
                    new { User = new UserEntity { FirstName = "Boba", LastName = "Gray", UserName = "user1", Email = "user@gmail.com" }, Password = "bobapass1", Role = Roles.User },
                    new { User = new UserEntity { FirstName = "Biba", LastName = "Undefined", UserName = "user2", Email = "biba@gmail.com" }, Password = "bibapass3", Role = Roles.User }
                };

            foreach (var i in users)
            {
                var outcome = await _userManager.CreateAsync(i.User, i.Password);

                if (!outcome.Succeeded) Console.WriteLine($"Failed to create user: {i.User.UserName}");
                else await _userManager.AddToRoleAsync(i.User, i.Role);
            }
        }
    }

}
}