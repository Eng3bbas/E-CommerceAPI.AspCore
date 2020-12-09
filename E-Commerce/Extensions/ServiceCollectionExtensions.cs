using AutoMapper;
using E_Commerce.Data;
using E_Commerce.Data.DTO;
using E_Commerce.Helpers;
using E_Commerce.Http.Middleware;
using E_Commerce.Services;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMyServices(this IServiceCollection services)
        {
            services.AddScoped<VerifyTokenNotRevoked>();
            services.AddAuthorization();
            services.AddScoped<TokenManger>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
            // Services
            
            services.AddScoped<UserService>();
            services.AddScoped<CategoriesService>();
            services.AddScoped<ProductsService>();
            services.AddScoped<OrdersService>();
        }
    }
}