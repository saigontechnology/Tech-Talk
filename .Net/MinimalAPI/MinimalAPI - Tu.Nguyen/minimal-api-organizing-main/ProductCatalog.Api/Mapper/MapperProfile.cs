using AutoMapper;
using ProductCatalog.Api.Commons;
using ProductCatalog.Api.Data.Entities;
using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserInformation>()
            .ForMember(dest => dest.RoleDisplay, option => option.Ignore())
            .ReverseMap();
        
        CreateMap<RegisterRequest, User>()
            .ForMember(dest => dest.Password, option => option.MapFrom(source => BCrypt.Net.BCrypt.HashPassword(source.Password)))
            .ForMember(dest => dest.CreatedBy, option => option.MapFrom(source => Constants.Admin))
            .ReverseMap();
        
        CreateMap<RegisterRequest, UserInformation>()
            .ReverseMap();
        
        CreateMap<Category, CategoryInformation>()
            .ReverseMap();
        
        CreateMap<Product, ProductInformation>()
            .ForMember(dest => dest.CategoryName, option => option.MapFrom(source => source.Category.Name))
            .ReverseMap();

        CreateMap<CreateProduct, Product>()
            .ForMember(dest => dest.CreatedBy, option => option.MapFrom(source => "Admin"))
            .ForMember(dest => dest.CreatedDate, option => option.Ignore())
            .ForMember(dest => dest.UpdatedBy, option => option.Ignore())
            .ForMember(dest => dest.UpdatedDate, option => option.Ignore())
            .ForMember(dest => dest.IsActive, option => option.Ignore());
        
        CreateMap<Product, CreateProductResponse>()
            .ReverseMap();

        CreateMap<UpdateProduct, Product>()
            .ForMember(dest => dest.UpdatedBy, option => option.MapFrom(source => "Admin"))
            .ForMember(dest => dest.UpdatedDate, option => option.MapFrom(source => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, option => option.Ignore())
            .ForMember(dest => dest.CreatedBy, option => option.Ignore())
            .ForMember(dest => dest.CreatedDate, option => option.Ignore());

        CreateMap<Product, UpdateProductResponse>()
            .ReverseMap();
    }
}