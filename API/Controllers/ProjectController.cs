using API.Services;
using DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectController)
        {
            _projectService = projectController;
        }

        [HttpPost("projects")]
        [Authorize]
        public async Task<IActionResult> CreateProject(ProjectDto dto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _projectService.CreateProject(dto,userId);

                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("projects/members")]
        [Authorize]
        public async Task<IActionResult> AddMemmberShip(MemberShipDto dto)
        {
            try
            {
                var loginedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _projectService.AddProjectMember(dto, loginedUserId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("projects/roles")]
        [Authorize]
        public async Task<IActionResult> AddRole(AddRoleWithPermissionDto dto)
        {
            try
            {
                await _projectService.AddRoleWithPermission(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("projects")]
        [Authorize]
        public async Task<IActionResult> GetProjects()
        {
            try
            {
                var loginedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                List<ProjectResult> result=  await _projectService.GetProjects(loginedUserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("projects/{id}/members")]
        [Authorize]
        public async Task<IActionResult> GetProjectMembers(long id)
        {
            try
            {
                var loginedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                List<ProjectMembersResult> result=  await _projectService.GetProjectMembers(id, loginedUserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("projects/roles")]
        [Authorize]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var loginedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                List<ProjectRolePermission> result = await _projectService.GetProjectRolesWithPermissions(loginedUserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("rols")]
        
        public async Task<IActionResult> GetRols()
        {
            try
            {
                var rols=await _projectService.GetRols();

                return Ok(rols);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
