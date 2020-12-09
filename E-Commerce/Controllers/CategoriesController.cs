using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using E_Commerce.Data.DTO;
using E_Commerce.Extensions;
using E_Commerce.Http;
using E_Commerce.Http.Responses;
using E_Commerce.Http.Responses.Categories;
using E_Commerce.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Controllers
{
    /// <inheritdoc />
    [ApiController]
    [Route("/api/[controller]/[action]")]
    [Authorize(Roles = "Admin" , AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(401,Type = typeof(string))]
    [Produces("application/json")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoriesService _service;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(CategoriesService service , ILogger<CategoriesController> logger)
        {
            _service = service;
            _logger = logger;
        }
        [HttpGet]
        [ActionName("")]
        [ProducesResponseType(200,Type = typeof(ListResponse))]
        [AllowAnonymous]
        public async Task<IActionResult> All()
        {
            return Ok(new
            {
                Data = await _service.GetAll()
            });
        }
        [HttpPost]
        [ProducesResponseType(400,Type = typeof(ErrorModel))]
        [ProducesResponseType(201,Type = typeof(SingleResponse))]
        public async Task<IActionResult> Create([Required,FromQuery] string name)
        {
            var response = await _service.Create(name);
            if (response.Status == BaseResponse.Statuses.Failed)
                return BadRequest(new
                {
                    Errors = new[] {new ErrorModel {Field = nameof(name), Errors = new[] {response.ErrorMessage}}}
                });
            return Created("",new
            {
                Data = response
            });
        }
        [HttpPut("{id:guid}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200,Type = typeof(SingleResponse))]
        public async Task<IActionResult> Update([Required,FromQuery] string name, Guid id)
        {
            var category = await _service.Update(name, id);
            if (category.Status == BaseResponse.Statuses.Failed)
                return NotFound();
            return Ok(new
            {
                Data = category
            });
        }
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _service.Delete(id))
            {
                return NotFound();
            }

            return NoContent();
        }
    }

    
}