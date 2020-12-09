using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using E_Commerce.Data.Entities;
using E_Commerce.Extensions;
using E_Commerce.Helpers.Pagination;

namespace E_Commerce.Data.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(u => u.Role, m => m.MapFrom(u => u.Role.ToString()));
            CreateMap<Category, CategoryDTO>();
            CreateMap<Product, ProductDTO>()
                .ForMember(dto => dto.Username, m => m.MapFrom(p => p.User.Name))
                .ForMember(dto => dto.Image , m => m.MapFrom(p => $"/Products/{p.Image}"))
                .ForMember(dto => dto.CreatedAt , m => m.MapFrom(p => p.CreatedAt.ToString("yyyy/M/d h:mm:ss")))
                .ForMember(dto => dto.Category, m => m.MapFrom(p => p.Category));
            CreateMap<Order, OrdersListDTO>()
                .ForMember(dto => dto.Username, m => m.MapFrom(o => o.User.Name))
                .ForMember(dto => dto.ProductsCount , m => m.MapFrom(o => o.Products.Count));
            CreateMap<Order, SingleOrderDTO>()
                .ForMember(dto => dto.User, m => m.MapFrom(o => o.User))
                .ForMember(dto => dto.Products, m => m.MapFrom(o => o.Products.Select(p => p.Product)));
            this.MapPagination<Product, ProductDTO>();
            this.MapPagination<Order,OrdersListDTO>();
        }
    }
}