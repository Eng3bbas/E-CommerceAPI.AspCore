using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using E_Commerce.Data;
using E_Commerce.Data.Entities;

namespace E_Commerce.Http.ValidationRules
{
    public class ProductIdAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var repository = validationContext.GetService(typeof(IRepository<Product>)) as IRepository<Product>;
            if (value is Guid)
            {
                return repository.Where(p => p.Id == Guid.Parse(value.ToString())).Any() ? 
                    ValidationResult.Success : new ValidationResult("Product Id not found");
            }

            if (value is IEnumerable<Guid> ids)
            {
                return repository.Where(p => ids.Contains(p.Id)).Count() == ids.Count() ?
                    ValidationResult.Success  : 
                    new ValidationResult("All products must be exists");
            }
            return base.IsValid(value, validationContext);
        }
    }
}