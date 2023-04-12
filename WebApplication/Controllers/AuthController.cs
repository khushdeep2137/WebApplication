using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApplication.Services;
using RequestModels = WebApplication.Contracts.Requests;
using ResponseModels = WebApplication.Contracts.Responses;


namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("/Login")]
        public async Task<IActionResult> Login([FromBody] RequestModels.UserLoginRequest request)
        {

            ResponseModels.AuthenticationResult response = new ResponseModels.AuthenticationResult();
            var user = await _authService.LoginAsync(request.Email, request.Password);
            if (user != null)
            {
                response = await _authService.GenerateJwtToken(user);

                if (!response.Success)
                {
                    response.Message = "Something Went Wrong";
                }
            }
            else
            {
                response.Message = "Invalid Credentials please try again.";
            }
            return Ok(response);
        }

    }
}
