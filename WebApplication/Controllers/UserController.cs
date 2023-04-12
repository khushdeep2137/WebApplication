using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication.EntityFilter;
using WebApplication.Services;
using RequestModels = WebApplication.Contracts.Requests;
using ResponseModels = WebApplication.Contracts.Responses;

namespace WebApplication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("/Users")]
        public async Task<IActionResult> GetAllUsers([FromBody] RequestModels.UserFilterRequest filter = null)
        {
            ResponseModels.ResultData resultData = new ResponseModels.ResultData();

            resultData = await _userService.SearchAsync<ResponseModels.UserResponse>(new UserFilter
            {
                PaginationFilter = filter.pagination,
                Keyword = filter.Keyword,
                SortingByDirection = filter.Sort.Direction,
                SortingByColumn = filter.Sort.Property
            });
            return Ok(resultData);
        }
    }
}
