using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using E_Commerce.Data;
using E_Commerce.Data.DTO;
using E_Commerce.Data.Entities;
using E_Commerce.Extensions;
using E_Commerce.Helpers.Pagination;
using E_Commerce.Http.Requests.Product;
using E_Commerce.Http.Responses.Products;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Services
{
    public class ProductsService
    {
        private readonly IRepository<Product> _repository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ProductsService> _logger;

        public ProductsService(IRepository<Product> repository, IMapper mapper, IWebHostEnvironment environment , ILogger<ProductsService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _environment = environment;
            _logger = logger;
        }

        public async Task<SingleProductResponse> Create(CreateRequest request, Guid userId)
        {
            if (!Directory.Exists(_environment.WebRootPath + "/Products"))
            {
                Directory.CreateDirectory(_environment.WebRootPath + "/Products");
            }

            string fileName = Guid.NewGuid() + Path.GetExtension(request.Image.FileName);
            using (var fs = new FileStream(Path.Combine(_environment.WebRootPath, "Products/", fileName),
                FileMode.Create))
            {
                await request.Image.CopyToAsync(fs);
            }

            var product = await _repository.Create(new Product
            {
                Name = request.Name,
                CategoryId = request.CategoryId,
                Image = fileName,
                Price = request.Price,
                UserId = userId,
                CreatedAt = DateTime.Now
            });
            await _repository.Save();
            return new SingleProductResponse
            {
                Product = _mapper.Map<ProductDTO>(product)
            };
        }

        public async Task<SingleProductResponse> Show(Guid id)
        {
            var product = await _repository.Find(id);
            if (product == null)
            {
                return new SingleProductResponse
                {
                    ErrorMessage = "Product not found"
                };
            }

            return new SingleProductResponse
            {
                Product = _mapper.Map<ProductDTO>(product)
            };
        }


        public async Task<ProductsListResponse> All(PaginationOptions options)
        {
            var products = await _repository.Paginate(options);
            return new ProductsListResponse
            {
                Data = _mapper.Map<PaginationList<ProductDTO>>(products)
            };
        }

        public async Task<bool> Delete(Guid id, Guid? userId)
        {
            var product = await _repository.Find(id);
            if (product == null)
            {
                return false;
            }

            if (product.UserId != userId)
            {
                return false;
            }

            await Task.Run(() => File.Delete(Path.Combine(_environment.WebRootPath, "Products/", product.Image)));
            await _repository.Delete(product);
            await _repository.Save();
            return true;
        }

        public async Task<SingleProductResponse> Update(UpdateRequest request, Guid id, Guid userId)
        {
            var product = _repository.Where(p => p.Id == id && p.UserId == userId).FirstOrDefault();
            if (product == null)
            {
                return new SingleProductResponse
                {
                    ErrorMessage = "Product not found"
                };
            }

            if (request.Image != null)
            {
                await Task.Run(() => File.Delete(Path.Combine(_environment.WebRootPath, "Products/", product.Image)));
                string fileName = Guid.NewGuid() + Path.GetExtension(request.Image.FileName);
                using (var stream = new FileStream(Path.Combine(_environment.WebRootPath, "Products/", fileName),
                    FileMode.Create))
                {
                    await request.Image.CopyToAsync(stream);
                }

                product.Image = fileName;
            }

            product.Name = request.Name;
            product.Price = request.Price;
            product.CategoryId = request.CategoryId;
            await _repository.Update(product);
            await _repository.Save();
            return new SingleProductResponse
            {
                Product = _mapper.Map<ProductDTO>(product)
            };
        }

        public async Task<ProductsListResponse> Filter(FilterRequest request, PaginationOptions paginationOptions)
        {
            try
            {
                var productsQuery = _repository
                    .Include(p => p.Category)
                    .Include(p => p.User)
                    .Where(p =>
                    p.Category.Name == request.CategoryName ||  request.CategoryId == p.CategoryId ||
                    request.UserId  == p.UserId);

                if (request.OrderType == FilterRequest.OrderByTypes.Desc)
                {
                    productsQuery = productsQuery.OrderByDescending(p => p.CreatedAt);
                }
                else
                {
                    productsQuery = productsQuery.OrderBy(p => p.CreatedAt);
                }
                
                return new ProductsListResponse
                {
                    Data = _mapper.Map<PaginationList<ProductDTO>>(await productsQuery.Paginate(paginationOptions))
                };
            }
            catch (InvalidOperationException e)
            {
                return new ProductsListResponse
                {
                    ErrorMessage = e.Message
                };
            }
        }
    }
}