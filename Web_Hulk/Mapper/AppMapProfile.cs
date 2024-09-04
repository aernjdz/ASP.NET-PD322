using AutoMapper;
using Web_Hulk.Data.Entities;
using Web_Hulk.Models.Categories;
using Web_Hulk.Models.Products;


namespace Web_Hulk.Mapper
{
    public class AppMapProfile : Profile
    {

        public AppMapProfile()
        {
            CreateMap<CategoryEntity, CategoryItemViewModel>();
            CreateMap<CategoryEntity, CategoryEditViewModel>();

            CreateMap<Product, ProductItemViewModel>()
           .ForMember(x => x.Images, opt => opt.MapFrom(x => x.ProductImages.Select(p => p.Image).ToArray()));

            CreateMap<Product, ProductEditViewModel>()
              .ForMember(x => x.Images, opt =>
              opt.MapFrom(src => src.ProductImages
              .Select(pi => new ProductImageViewModel
              {
                  Id = pi.Id,
                  Name = "/images/" + pi.Image,
                  Priority = pi.Priority
              }).ToList()));

            CreateMap<ProductEditViewModel, Product>()
                .ForMember(x => x.Id, opt => opt.Ignore());

        }
    }
}
