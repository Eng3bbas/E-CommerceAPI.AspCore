using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using E_Commerce.Data.DTO;
using E_Commerce.Helpers.Pagination;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Extensions
{
    public static class PaginationExtensions
    {
        public static async Task<PaginationList<TEntity>> Paginate<TEntity>(this IQueryable<TEntity> query,PaginationOptions options) where TEntity : class
        {
            if (query == null)
            {
                throw new ArgumentException("Error , query can not be null" , nameof(query));
            }
            int count = await query.CountAsync();
            if (count == 0)
            {
                return new PaginationList<TEntity>
                {
                    Rows = new List<TEntity>(),
                    Metadata = new PaginationMetadata
                    {
                        PerPage = options.PerPage,
                        RowsCount = 0,
                        PagesCount = 0
                    }
                };
            }
            return new PaginationList<TEntity>
            {
                Rows = await query.Skip((options.Page - 1) * options.PerPage).Take(options.PerPage).ToListAsync(),
                Metadata = new PaginationMetadata
                {
                    PerPage = options.PerPage,
                    PagesCount = (int) Math.Ceiling(count / (decimal) options.PerPage),
                    RowsCount = count
                }
            };
        }
        public static IMappingExpression<PaginationList<TEntityPagination>,PaginationList<TDTOPagination>> MapPagination<TEntityPagination,TDTOPagination>(this MappingProfile mapper) where TEntityPagination : class where TDTOPagination : class
        {
            return mapper.CreateMap<PaginationList<TEntityPagination>, PaginationList<TDTOPagination>>()
                .ForMember(d => d.Rows, options => options.MapFrom(o => o.Rows));
        }
    }
}