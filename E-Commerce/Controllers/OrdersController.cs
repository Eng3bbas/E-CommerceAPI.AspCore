using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using E_Commerce.Data;
using E_Commerce.Data.DTO;
using E_Commerce.Data.Entities;
using E_Commerce.Extensions;
using E_Commerce.Helpers.Pagination;
using E_Commerce.Http;
using E_Commerce.Http.Requests.Order;
using E_Commerce.Http.Responses;
using E_Commerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Controllers
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(400,Type = typeof(ErrorModel))]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersService _service;

        public OrdersController(OrdersService service)
        {
            _service = service;
        }
        [ActionName("")]
        [HttpGet]
        [ProducesResponseType(200,Type = typeof(PaginationList<OrdersListDTO>))]
        public async Task<ActionResult<PaginationList<OrdersListDTO>>> Index([FromQuery] PaginationOptions paginationOptions , [FromServices] IRepository<User> userRepository)
        {
            var userRole = userRepository.Where(u => u.Id == HttpContext.User.GetUserId().Value).Select(u => u.Role).SingleOrDefault();
            if (userRole == Data.Entities.User.Roles.Admin)
            {
                return await _service.All(paginationOptions);
            }
            return await _service.All(options: paginationOptions, userId: User.GetUserId().Value);
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        [ProducesResponseType(201,Type = typeof(SingleOrderDTO))]
        public async Task<IActionResult> Create([FromBody] NewOrder request)
        {
            return Created("",await _service.Create(request, User.GetUserId().Value));
        }
        [HttpGet("{id:guid}")]
        [ProducesResponseType(200,Type = typeof(SingleOrderDTO))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Show(Guid id)
        {
            var response = User.GetUserRole().Value == Data.Entities.User.Roles.User
                ? await _service.Show(id, User.GetUserId().Value)
                : await _service.Show(id);
            if (response.ErrorMessage != null)
            {
                return NotFound(new
                {
                    Error = response.ErrorMessage
                });
            }
            
            return Ok(response.Order);
        }
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200,Type = typeof(SingleOrderDTO))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<SingleOrderDTO>> Update(Guid id, [FromBody] UpdateStatus request)
        {
            var response = await _service.UpdateStatus(id, request.Status);
            if (response.Status == BaseResponse.Statuses.Failed)
            {
                return NotFound(response);
            }
            return response.Order;
        }
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool orderStatus;
            if (User.GetUserRole() == Data.Entities.User.Roles.Admin)
            {
                orderStatus = await _service.Delete(id);
                if (!orderStatus)
                {
                    return NotFound();
                }

                return NoContent();
            }
            orderStatus = await _service.Delete(id,User.GetUserId().Value);
            if (!orderStatus)
            {
                return NotFound();
            }

            return NoContent();
        }
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<PaginationList<OrdersListDTO>>> Filter([FromQuery] FilterOrders request)
        {
            if (User.GetUserRole() == Data.Entities.User.Roles.Admin)
            {
                return await _service.Filter(request);
            }

            return await _service.Filter(request, User.GetUserId().Value);
        }
    }
}