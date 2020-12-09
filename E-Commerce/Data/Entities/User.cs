using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AutoMapper.Mappers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;

namespace E_Commerce.Data.Entities
{
    public class User : BaseEntity
    {
        public enum Roles
        {
            Admin,
            User
        }
        [Required]
        public string Name { get; set; }
        [Display(Name = "PasswordHash"),Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        
        public Roles Role { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}