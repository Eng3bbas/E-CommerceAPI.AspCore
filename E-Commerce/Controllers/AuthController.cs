using System;
using System.Linq;
using System.Threading.Tasks;
using E_Commerce.Extensions;
using E_Commerce.Helpers;
using E_Commerce.Http;
using E_Commerce.Http.Requests;
using E_Commerce.Http.Responses;
using E_Commerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _service;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserService service,ILogger<AuthController> logger)
        {
            _service = service;
            _logger = logger;
        }
        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status200OK , Type = typeof(AuthenticationResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(ErrorModel[]))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var attemptRegister = await _service.Register(request);
                if (attemptRegister.Status == BaseResponse.Statuses.Failed)
                    return BadRequest(new
                    {
                        Errors = new []
                        {
                            new ErrorModel { Field = "Email" , Errors = new [] {attemptRegister.ErrorMessage}}
                        }
                    });
                return Ok(new {Data = new {attemptRegister.Token, attemptRegister.User}});
            }
            catch (Exception e)
            {
                _logger.LogError("exception thrown  : {0}" , e.Message);
                return StatusCode(500, new {ServerError = e.Message});
            }
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(AuthenticationResponse))]
        
        public async Task<IActionResult> Login( [FromBody] LoginRequest request)
        {
            var loginAttempt = await _service.Login(request);
            if (loginAttempt.Status == BaseResponse.Statuses.Failed)
                return Unauthorized(new {AuthErrorMessage = loginAttempt.ErrorMessage});
            return Ok(new {Data = new {loginAttempt.Token, loginAttempt.User}});
        }
        [HttpPost("Logout")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Logout([FromServices] TokenManger tokenManger)
        {
            await tokenManger.Revoke(Guid.Parse(User.FindFirst("jti").Value), User.GetUserId().Value );
            return NoContent();
        }
    }
}