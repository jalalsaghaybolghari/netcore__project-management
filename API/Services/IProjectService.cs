using DAL.Entity;
using DAL.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public interface IProjectService
    {
        Task AddRoleWithPermission(AddRoleWithPermissionDto dto);
        Task AddProjectMember(MemberShipDto dto,string loginedUserId);
        Task CreateProject(ProjectDto dto, string userId);
        Task<List<ProjectMembersResult>> GetProjectMembers(long id, string loginedUserId);
        Task<List<ProjectRolePermission>> GetProjectRolesWithPermissions(string userId);
        Task<List<ProjectResult>> GetProjects(string loginedUserId);
        Task<List<RolsList>> GetRols();
        Task<bool> HasPermission(string userId, long projectId, string methodName);
    }

    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> userManager;
        public ProjectService(ApplicationDbContext context
            , UserManager<ApplicationUser> userManager)
        {
            _db = context;
            this.userManager = userManager;
        }
        public async Task AddRoleWithPermission(AddRoleWithPermissionDto dto)
        {
            ProjectRole role;

            var existedRole = await _db.ProjectRoles.FirstOrDefaultAsync(c => c.Title == dto.RoleTitle);

            if (existedRole != null)
            {
                role = existedRole;
            }
            else
            {
                role = new ProjectRole
                {
                    Title = dto.RoleTitle,
                    RolePermissions = new List<RolePermission>()
                };
            }
            var rolePermissions = new List<RolePermission>();

            if (dto.PermitedActions.Any())
            {
                foreach (var action in dto.PermitedActions)
                {
                    rolePermissions.Add(new RolePermission { Action = action });
                }
            }

            role.RolePermissions?.AddRange(rolePermissions);

            await _db.ProjectRoles.AddAsync(role);

            await _db.SaveChangesAsync();

        }

        public async Task AddProjectMember(MemberShipDto dto, string loginedUserId)
        {
            var user = await userManager.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                throw new Exception("Inserted userName not found!");
            }

            if (string.IsNullOrEmpty(loginedUserId) ||
                !await HasPermission(loginedUserId, dto.ProjectId, "AddMemmberShip"))
            {
                throw new Exception("Access denied");
            }
            var projectMember = new ProjectMember
            {
                ProjectId = dto.ProjectId,
                FromDate = dto.FromDate,
                ProjectRoleId = 2,
                IsDelete = false,
                ToDate = dto.ToDate,
                ApplicationUserId = user.Id
            };

            await _db.ProjectMembers.AddAsync(projectMember);
            await _db.SaveChangesAsync();
        }

        public async Task CreateProject(ProjectDto dto, string userId)
        {
            var project = new Project
            {
                Title = dto.Title,
                Description = dto.Description,
                EndDate = dto.EndDate,
                StartDate = dto.StartDate,
            };

            var projectMember = new ProjectMember
            {
                FromDate = DateTime.Now,
                ProjectRoleId = 1,
                IsDelete = false,
                ToDate = null,
                ApplicationUserId = userId
            };

            project.ProjectMembers = new List<ProjectMember> { projectMember };
            await _db.Projects.AddAsync(project);
            await _db.SaveChangesAsync();
        }

        public async Task<List<ProjectMembersResult>> GetProjectMembers(long id,string loginedUserId)
        {
            if (string.IsNullOrEmpty(loginedUserId) ||
                !await HasPermission(loginedUserId, id, "GetProjectMembers"))
            {
                throw new Exception("Access denied");
            }
            return await _db.ProjectMembers.Where(c => c.ProjectId == id).Select(c => new ProjectMembersResult
            {
                UserName = c.ApplicationUser.UserName,
                FromDate = c.FromDate,
                Id = c.Id,
                ProjectRole = c.ProjectRole.Title,
                ToDate = c.ToDate
            }).ToListAsync();
        }

        public async Task<List<ProjectRolePermission>> GetProjectRolesWithPermissions(string userId)
        {
            return await _db.ProjectMembers.Where(p => p.ApplicationUserId == userId)
                .Select(c => new ProjectRolePermission
                {
                    RoleId=c.ProjectRoleId,
                    ProjectId=c.ProjectId,
                    RoleTitle=c.ProjectRole.Title,
                    ProjectTitle= c.Project.Title
                    //PermitedActions=c.ProjectRole.RolePermissions.Select(c => c.Action).ToList()
                }).ToListAsync();
        }

        public async Task<List<ProjectResult>> GetProjects(string loginedUserId)
        {
            var projects = _db.Projects
                .Include(c => c.ProjectMembers)
                .Include(c => c.ProjectMembers).ThenInclude(c => c.ProjectRole)
                .Where(p => p.ProjectMembers
                .Any(pm => pm.ApplicationUserId == loginedUserId))
                .Select(c => new ProjectResult
                {
                    Id = c.Id,
                    Description = c.Description,
                    EndDate = c.EndDate,
                    StartDate = c.StartDate,
                    Title = c.Title
                });
            return await projects.ToListAsync();
        }

        public async Task<List<RolsList>> GetRols()
        {
            var rols = await _db.ProjectRoles.Include(c => c.RolePermissions)
                .Select(v => new RolsList
                {
                    Id = v.Id,
                    Title = v.Title,
                    Actions = v.RolePermissions.Select(v => v.Action).ToList()

                }).ToListAsync();
            return rols;
        }

        public async Task<bool> HasPermission(string userId, long projectId, string methodName)
        {
            return await _db.ProjectMembers.AnyAsync(c => c.ApplicationUserId == userId &&
                                                         c.ProjectId == projectId &&
                                                         (c.ProjectRole.RolePermissions
                                                         .Select(v => v.Action).Contains(methodName)));
        }
    }
}
