using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorklogTrackingSystem.Application.Interfaces;
using WorklogTrackingSystem.Domain.DTOs;
using WorklogTrackingSystem.Domain.Entities;
using WorklogTrackingSystem.Domain.Enums;

namespace WorklogTrackingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost("register")]
        [Authorize(Roles = $"{nameof(Role.Admin)}")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var user = await _userService.RegisterAsync(request);

            if (user is null) return BadRequest("Username already exists.");

            return Ok(user);
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = $"{nameof(Role.Admin)}")]
        public async Task<ActionResult<User>> Update(int id, UserDto request)
        {
            var user = await _userService.UpdateAsync(id, request);

            if (user is null) return NotFound("User not found.");

            return Ok(user);
        }

        [HttpGet("paged-users")]
        [Authorize(Roles = $"{nameof(Role.Admin)}")]
        public async Task<ActionResult<PagedResultDto<UserDto>>> GetPagedUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Page and pageSize must be greater than 0.");
            }

            var pagedResult = await _userService.GetPagedAsync(page, pageSize);

            return Ok(pagedResult);
        }
    }
}