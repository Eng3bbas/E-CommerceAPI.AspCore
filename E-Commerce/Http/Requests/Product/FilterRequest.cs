using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using AutoMapper.Mappers;
using E_Commerce.Helpers.Pagination;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace E_Commerce.Http.Requests.Product
{
    public class FilterRequest
    {
        public FilterRequest()
        {
          new EnumToStringConverter<OrderByTypes>();
        }
        public enum OrderByTypes
        {
            [EnumDataType(typeof(string))]
            Desc,
            [EnumDataType(typeof(string))]
            Asc
        }
        public string CategoryName { get; set; }
        public Guid CategoryId { get; set; }
        public Guid UserId { get; set; }
        [Required]
        [JsonConverter(typeof(EnumToStringConverter<OrderByTypes>))]
        public OrderByTypes OrderType { get; set; }
        public PaginationOptions PaginationOptions { get; set; } = new PaginationOptions();
    }
}