using System;
using System.ComponentModel.DataAnnotations;
using E_Commerce.Data;
using E_Commerce.Data.Entities;
using E_Commerce.Http.ValidationRules;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.Http.Requests.Product
{
    public class CreateRequest
    {
        [Required, StringLength(255)] public string Name { get; set; }
        [Required] public IFormFile Image { get; set; }
        [Range(1, 10000000), Required] public double Price { get; set; }
        [Required, CategoryId] public Guid CategoryId { get; set; }
        [FileExtensions] private string ImageExtensions => Image.FileName;
    }
}