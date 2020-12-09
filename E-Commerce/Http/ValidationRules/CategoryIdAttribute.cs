using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using E_Commerce.Data;
using E_Commerce.Data.Entities;

namespace E_Commerce.Http.ValidationRules
{
    public class CategoryIdAttribute : ValidationAttribute
    {
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("CategoryId is required");
            }
            var repository = validationContext.GetService(typeof(IRepository<Category>)) as IRepository<Category>;
            var categoryId = Guid.Parse(value.ToString());
            return repository.Where(c => c.Id == categoryId).Any() ? 
                ValidationResult.Success : 
                new ValidationResult("Category Id is not found");
        }
    }
}