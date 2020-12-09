using System;
using System.Threading.Tasks;
using E_Commerce.Data.DTO;
using E_Commerce.Extensions;
using E_Commerce.Helpers.Pagination;
using E_Commerce.Http;
using E_Commerce.Http.Requests.Product;
using E_Commerce.Http.Responses;
using E_Commerce.Http.Responses.Products;
using E_Commerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(401,Type = typeof(Nullable))]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsService _service;
        
        public ProductsController(ProductsService service)
        {
            _service = service;
        }
        [HttpPost]
        [ProducesResponseType(201,Type = typeof(SingleProductResponse))]
        [ProducesResponseType(400,Type = typeof(ErrorModel[]))]
        [ProducesResponseType(401,Type = typeof(string))]
        public async Task<IActionResult> Create([FromForm] CreateRequest request)
        {
            var userId = User.GetUserId();
            if (!userId.HasValue)
            {
                return Unauthorized();
            }
            var product = await _service.Create(request, userId.Value);
            return Created("",new
            {
                Data = product
            });
        }
        [HttpGet]
        [ActionName("")]
        [ProducesResponseType(200,Type = typeof(PaginationList<ProductDTO>))]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery] PaginationOptions options)
        {
            return Ok(await _service.All(options));
        }
        [HttpGet("{id:guid}")]
        [ProducesResponseType(200,Type = typeof(ProductDTO))]
        [ProducesResponseType(404,Type = typeof(string))]
        [AllowAnonymous]
        public async Task<IActionResult> Show(Guid id)
        {
            var response = await _service.Show(id);
            if (response.Status == BaseResponse.Statuses.Failed)
            {
                return NotFound(response.ErrorMessage);
            }

            return Ok(new
            {
                response.Product
            });
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool isDeleted = await _service.Delete(id, User.GetUserId());
            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(200,Type = typeof(SingleProductResponse))]
        [ProducesResponseType(404,Type = typeof(string))]
        public async Task<IActionResult> Update([FromForm] UpdateRequest request, Guid id)
        {
            var response = await _service.Update(request, id,User.GetUserId().Value);
            if (response.Status == BaseResponse.Statuses.Failed)
            {
                return NotFound(response.ErrorMessage);
            }

            return Ok(response);
        }
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200,Type = typeof(ProductsListResponse))]
        public async Task<IActionResult> Filter([FromQuery] FilterRequest request)
        {
            return Ok(await _service.Filter(request, request.PaginationOptions));
        }
    }
}