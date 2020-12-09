using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using E_Commerce.Data;
using E_Commerce.Data.DTO;
using E_Commerce.Data.Entities;
using E_Commerce.Http.Requests;
using E_Commerce.Http.Responses.Categories;

namespace E_Commerce.Services
{
    public class CategoriesService
    {
        private readonly IRepository<Category> _repository;
        private readonly IMapper _mapper;

        public CategoriesService(IRepository<Category> repository , IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ListResponse> GetAll()
        {
            return new ListResponse
            {
                Categories = _mapper.Map<List<CategoryDTO>>(await _repository.GetAll())
            };
        }

        public async Task<SingleResponse> Create(string name)
        {
            if (_repository.Where(c => c.Name == name).Any())
                return new SingleResponse
                {
                    ErrorMessage = "category name is already exists"
                };
            var category = await _repository.Create(new Category
            {
                Name = name
            });
            await _repository.Save();
            return new SingleResponse
            {
                Category = _mapper.Map<CategoryDTO>(category)
            };
        }
        
        public async Task<SingleResponse> Update(string name , Guid id)
        {
            var category = await _repository.Find(id);
            if (category == null)
            {
                return new SingleResponse
                {
                    ErrorMessage = "category not found"
                };
            }
            category.Name = name;
            await _repository.Update(category);
            await _repository.Save();
            return new SingleResponse
            {
                Category = _mapper.Map<CategoryDTO>(category)
            };
        }

        public async Task<bool> Delete(Guid id)
        {
            var category = await _repository.Find(id);
            if (category == null)
            {
                return false;
            }
            await _repository.Delete(category);
            await _repository.Save();
            return true;
        }
    }
}