using AutoMapper;
using System.Globalization;
using Web_Hulk.Areas.Admin.Models.Categories;
using Web_Hulk.Areas.Admin.Models.Products;
using Web_Hulk.Data.Entities.Identity;
using Web_Hulk.Data.Entities;
using Web_Hulk.Models.Account;
using Web_Hulk.Models.Products;

public class AppMapProfile : Profile
{
    public AppMapProfile()
    {
        CreateMap<CategoryEntity, CategoryItemViewModel>();
        CreateMap<CategoryEntity, Web_Hulk.Models.Categories.CategoryItemViewModel>();
        CreateMap<CategoryEntity, CategoryEditViewModel>();

        CreateMap<Product, Web_Hulk.Models.Products.ProductItemViewModel>()
            .ForMember(x => x.Images, opt => opt.MapFrom(x => x.ProductImages.Select(p => p.Image).ToArray()));

        CreateMap<Product, Web_Hulk.Areas.Admin.Models.Products.ProductItemViewModel>()
            .ForMember(x => x.Images, opt => opt.MapFrom(x => x.ProductImages.Select(p => p.Image).ToArray()));

        CreateMap<Product, ProductEditViewModel>()
          .ForMember(x => x.Images, opt =>
          opt.MapFrom(src => src.ProductImages
          .Select(pi => new Web_Hulk.Areas.Admin.Models.Products.ProductImageViewModel
          {
              Id = pi.Id,
              Name = "/images/" + pi.Image,
              Priority = pi.Priority
          }).ToList()))
                .ForMember(x => x.Price, opt => opt.MapFrom(x => x.Price.ToString(new CultureInfo("uk-UA"))));

        CreateMap<ProductEditViewModel, Product>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Price, opt => opt.MapFrom(x => Decimal.Parse(x.Price, new CultureInfo("uk-UA"))));

        CreateMap<UserEntity, ProfileViewModel>()
            .ForMember(x => x.FullName, opt => opt.MapFrom(x => $"{x.FirstName} {x.LastName}"));

    }
}