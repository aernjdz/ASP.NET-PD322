using StoreApi.Models.Category;
using StoreApi.Models.Product;
using StoreApi.Data.Entities;
using AutoMapper;

namespace StoreApi.Mapper
{
    public class AppMapProfile : Profile
    {
        public AppMapProfile()
        {
            // Categories
            CreateMap<CategoryCreateViewModel, CategoryEntity>()
                .ForMember(x => x.Image, opt => opt.Ignore());

            CreateMap<CategoryEditViewModel, CategoryEntity>()
                .ForMember(x => x.Image, opt => opt.Ignore());

            CreateMap<CategoryEntity, CategoryItemViewModel>();

            CreateMap<CategoryEntity, SelectItemViewModel>()
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            // Products
            CreateMap<ProductEntity, ProductItemViewModel>()
                          .ForMember(x => x.Images, opt => opt.MapFrom(x => x.ProductImages
                              .Select(p => p.Image).ToArray()));

            CreateMap<ProductCreateViewModel, ProductEntity>();
            CreateMap<ProductEditViewModel, ProductEntity>()
                .ForMember(x => x.ProductImages, opt => opt.Ignore());

        }
    }
}