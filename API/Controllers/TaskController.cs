using API.Services;
using DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskManagerService _taskManagerService;
        public TaskController(ITaskManagerService taskManagerService)
        {
            _taskManagerService = taskManagerService;
        }
        [HttpPost("tasks")]
        [Authorize]
        public async Task<IActionResult> CreateTask(TaskDto dto)
        {
            try
            {
                var loginedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                long taskId= await _taskManagerService.CreateTask(dto,loginedUserId);

                return Ok(taskId);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("tasks/assign-to-member")]
        [Authorize]
        public async Task<IActionResult> AssignTask(TaskAssignDto dto)
        {
            try
            {
                var loginedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _taskManagerService.AssignTask(dto, loginedUserId);

                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut("tasks/change-priority")]
        [Authorize]
        public async Task<IActionResult> ChangeTaskPriority(TaskPriorityDto dto)
        {
            try
            {
                var loginedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _taskManagerService.ChangePriority(dto,loginedUserId);

                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("tasks/{taskId}/status")]
        [Authorize]
        public async Task<IActionResult> GetTaskStatuses(long taskId)
        {
            try
            {
                List<TaskStatusResult> resul = await _taskManagerService.GetTaskStatus(taskId);

                return Ok(resul);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut("tasks/status")]
        [Authorize]
        public async Task<IActionResult> ChangeTaskStatuses(ChangeTaskStatusDto dto)
        {
            try
            {
                var loginedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _taskManagerService.ChangeTaskStatuses(dto,loginedUserId);

                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("tasks")]
        [Authorize]
        public async Task<IActionResult> GetFilteredTasks([FromQuery]TaskFilter dto)
        {
            try
            {
                var loginedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                List<FilteredTaskResult> results =  await _taskManagerService.GetFilteredTasks(dto, loginedUserId);

                return Ok(results);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
