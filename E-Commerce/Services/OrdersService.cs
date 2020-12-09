using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Commerce.Data;
using E_Commerce.Data.DTO;
using E_Commerce.Data.Entities;
using E_Commerce.Extensions;
using E_Commerce.Helpers.Pagination;
using E_Commerce.Http.Requests.Order;
using E_Commerce.Http.Responses.Orders;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Services
{
    
    public class OrdersService
    {
        private readonly IRepository<Order> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<OrdersService> _logger;

        public OrdersService(IRepository<Order> repository , IMapper mapper , ILogger<OrdersService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaginationList<OrdersListDTO>> All(PaginationOptions options)
        {
            _logger.LogInformation("Orders as admin fetched at :{0}",DateTime.Now.ToString());
            return _mapper.Map<PaginationList<OrdersListDTO>>(await _repository.Paginate(options));
        }
        
        public async Task<PaginationList<OrdersListDTO>> All(PaginationOptions options, Guid userId)
        {
            _logger.LogInformation("orders as user {0} fetched at {1}",userId,DateTime.Now);
            return _mapper.Map<PaginationList<OrdersListDTO>>(await _repository.Where(o => o.UserId == userId).Paginate(options));
        }

        public async Task<SingleOrderDTO> Create(NewOrder request , Guid userId)
        {
            var order = new Order
            {
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                UserId = userId,
                Status = Order.Statuses.Pending,
                CreatedAt = DateTime.Now
            };
            order.Products = request.Products.Select(pId => new OrderProduct
            {
                    OrderId = order.Id,
                    ProductId = pId
            }).ToArray();
            await _repository.Create(order);
            await _repository.Save();
            _logger.LogInformation("new order created with id : {0}",order.Id);
            return _mapper.Map<SingleOrderDTO>(order);
        }

        public async Task<SingleOrderResponse> Show(Guid id)
        {
            var order = await  _repository.Find(id);
            if (order == null)
            {
                return new SingleOrderResponse
                {
                    ErrorMessage =  "Order not found"
                };
            }
            return new SingleOrderResponse
            {
                Order = _mapper.Map<SingleOrderDTO>(order)
            };
        }

        public async Task<SingleOrderResponse> Show(Guid id, Guid userId)
        {
            var order = _repository.Where(o => o.UserId == userId && o.Id == id).FirstOrDefault();
            if (order == null)
            {
                return new SingleOrderResponse
                {
                    ErrorMessage = "Order not found"
                };
            }
            return new SingleOrderResponse
            {
                Order = _mapper.Map<SingleOrderDTO>(order)
            };
        }

        public async Task<SingleOrderResponse> UpdateStatus(Guid id, Order.Statuses status)
        {
            var order = await _repository.Find(id);
            if (order == null)
            {
                return new SingleOrderResponse
                {
                    ErrorMessage = "Order not found"
                };
            }
            order.Status = status;
            await _repository.Update(order);
            await _repository.Save();
            return new SingleOrderResponse
            {
                Order = _mapper.Map<SingleOrderDTO>(order)
            };
        }

        public async Task<bool> Delete(Guid id)
        {
            var order = await _repository.Where(o => o.Id == id && o.Status != Order.Statuses.Deleted).FirstOrDefaultAsync();
            if (order == null)
            {
                return false;
            }
            order.Status = Order.Statuses.Deleted;
            await _repository.Update(order);
            await _repository.Save();
            return true;
        }

        public async Task<bool> Delete(Guid id, Guid userId)
        {
            var order = await _repository.Where(o => o.Status != Order.Statuses.Deleted && o.UserId == userId && o.Id == id && o.CreatedAt.Hour - DateTime.Now.Hour <= 1).FirstOrDefaultAsync();
            if (order == null)
            {
                return false;
            }

            order.Status = Order.Statuses.Deleted;
            await _repository.Update(order);
            await _repository.Save();
            return true;
        }

        public async Task<PaginationList<OrdersListDTO>> Filter(FilterOrders request)
        {
            var query = _repository
                .Where(o => o.Address == request.Address || o.PhoneNumber == request.Phone ||
                            o.Status == request.OrderStatus);
            if (request.OrderingBy == FilterOrders.OrderingByCreatedAtTypes.Desc)
            {
                query = query.OrderByDescending(o => o.CreatedAt);
            }
            else
            {
                query = query.OrderBy(o => o.CreatedAt);
            }
            return _mapper.Map<PaginationList<OrdersListDTO>>(await query.Paginate(request.PaginationOptions));
        }
        public async Task<PaginationList<OrdersListDTO>> Filter(FilterOrders request , Guid userId)
        {
            var query = _repository
                    .Where(o => o.UserId == userId && (o.Address == request.Address || o.PhoneNumber == request.Phone ||
                                                       o.Status == request.OrderStatus))
                ;
            if (request.OrderingBy == FilterOrders.OrderingByCreatedAtTypes.Desc)
            {
                query = query.OrderByDescending(o => o.CreatedAt);
            }
            else
            {
                query = query.OrderBy(o => o.CreatedAt);
            }
            return _mapper.Map<PaginationList<OrdersListDTO>>(await query.Paginate(request.PaginationOptions));
        }
    }
}