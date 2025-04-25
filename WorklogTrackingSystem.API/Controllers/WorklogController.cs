using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorklogTrackingSystem.Application.Interfaces;
using WorklogTrackingSystem.Domain.DTOs;
using WorklogTrackingSystem.Domain.Entities;
using WorklogTrackingSystem.Domain.Enums;
using WorklogTrackingSystem.Infrastructure.Services;

namespace WorklogTrackingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorklogController(IWorklogService worklogService) : ControllerBase
    {
        private readonly IWorklogService _worklogService = worklogService;

        [HttpPost("insert")]
        [Authorize(Roles = $"{nameof(Role.User)}")]
        public async Task<ActionResult<Worklog>> Insert(WorklogDto worklog)
        {
            var newWorklog = await _worklogService.InsertAsync(worklog);

            if (newWorklog is null) return BadRequest("Failed to insert worklog.");

            return Ok(newWorklog);
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = $"{nameof(Role.User)}")]
        public async Task<ActionResult<Worklog>> Update(int id, WorklogDto worklog)
        {
            var updatedWorklog = await _worklogService.UpdateAsync(id, worklog);

            if (updatedWorklog is null) return NotFound("Worklog not found.");

            return Ok(updatedWorklog);
        }

        [HttpGet("summary/{userId}")]
        [Authorize(Roles = $"{nameof(Role.User)},{nameof(Role.Admin)}")]
        public async Task<ActionResult<PagedResultDto<WorklogSummaryDto>>> GetSummary(int userId, int page = 1, int pageSize = 10, string? filter = null)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Page and pageSize must be greater than 0.");
            }

            var pagedResult = await _worklogService.GetWorklogSummaryByDateAsync(userId, page, pageSize, filter);

            return Ok(pagedResult);
        }
    }
}