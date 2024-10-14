using DAL.Entity;
using DAL.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace API.Services
{
    public interface ITaskManagerService
    {
        Task AssignTask(TaskAssignDto dto,string loginUserId);
        Task ChangePriority(TaskPriorityDto dto, string loginUserId);
        Task ChangeTaskStatuses(ChangeTaskStatusDto dto, string loginUserId);
        Task<long> CreateTask(TaskDto dto,string loginUserId);
        Task<List<FilteredTaskResult>> GetFilteredTasks(TaskFilter dto, string loginUserId);
        Task<List<TaskStatusResult>> GetTaskStatus(long taskId);
    }

    public class TaskManagerService : ITaskManagerService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> userManager;
        public TaskManagerService(ApplicationDbContext context
                                  , UserManager<ApplicationUser> userManager)
        {
            _db = context;
            this.userManager = userManager;
        }

        public async Task AssignTask(TaskAssignDto dto, string loginUserId)
        {
            var task = await _db.ProjectTasks.FirstOrDefaultAsync(c => c.Id == dto.TaskId);

            if (task == null)
            {
                throw new Exception("Task not found!");
            }

            if (string.IsNullOrEmpty(loginUserId) ||
                    !await HasPermission(loginUserId, task.ProjectId.Value, "AssignTask"))
            {
                throw new Exception("Access denied");
            }

            var user = await userManager.FindByNameAsync(dto.UserName);
            if (user == null)
            {
                throw new Exception("Inserted userName not found!");
            }

            var taskMember = new TaskMember
            {
                ApplicationUserId = user.Id,
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                IsDelete = false,
                ProjectTaskId = task.Id,
                Responsibility = dto.Responsibility
            };

            await _db.TaskMembers.AddAsync(taskMember);
            await _db.SaveChangesAsync();
        }

        public async Task ChangePriority(TaskPriorityDto dto, string loginUserId)
        {
            var task = await _db.ProjectTasks.FirstOrDefaultAsync(c => c.Id == dto.TaskId);           

            if (task == null)
            {
                throw new Exception("Task not found!");
            }
            if (string.IsNullOrEmpty(loginUserId) ||
                    !await HasPermission(loginUserId, task.ProjectId.Value, "ChangeTaskPriority"))
            {
                throw new Exception("Access denied");
            }
            task.Priority = dto.Priority;

            await _db.SaveChangesAsync();
        }

        public async Task ChangeTaskStatuses(ChangeTaskStatusDto dto, string loginUserId)
        {
            var task =await _db.ProjectTasks.FirstOrDefaultAsync(t => t.Id == dto.TaskId);

            if (task == null)
            {
                throw new Exception("Task not found!");
            }
            if (string.IsNullOrEmpty(loginUserId) ||
                    !await HasPermission(loginUserId, task.ProjectId.Value, "ChangeTaskStatuses"))
            {
                throw new Exception("Access denied");
            }

            var taskStatus= await _db.TaskStatuses.OrderByDescending(c=>c.Time).FirstOrDefaultAsync(c=>c.ProjectTaskId == dto.TaskId);
            if (taskStatus != null && taskStatus.Status==dto.TaskStatus)
            {
                throw new Exception("Task status is same as before!");
            }
            var status = new DAL.Entity.TaskStatus
            {
                ProjectTaskId = dto.TaskId,
                Status = dto.TaskStatus,
                Time = DateTime.Now
            };
            await _db.TaskStatuses.AddAsync(status);

            await _db.SaveChangesAsync();
        }

        public async Task<long> CreateTask(TaskDto dto, string loginUserId)
        {
            if (string.IsNullOrEmpty(loginUserId) ||
                    !await HasPermission(loginUserId, dto.ProjectId, "CreateTask"))
            {
                throw new Exception("Access denied");
            }
            var task = new ProjectTask
            {
                Title = dto.Title,
                Description = dto.Description,
                DeadLine = dto.DeadLine,
                Priority = dto.Priority,
                ProjectId = dto.ProjectId
            };

            await _db.ProjectTasks.AddAsync(task);

            await _db.SaveChangesAsync();

            return task.Id;
        }

        public async Task<List<FilteredTaskResult>> GetFilteredTasks(TaskFilter dto, string loginUserId)
        {
            var userTasks = _db.ProjectTasks.Where(i => i.Project.ProjectMembers.Any(m => m.ApplicationUserId == loginUserId));
            var query = userTasks.Select(c=> new FilteredTaskResult
            {
                Id= c.Id,
                DeadLine= c.DeadLine,
                Description= c.Description,
                Priority = c.Priority,
                ProjectId= c.ProjectId,
                Title=c.Title,
                ProjectTitle = c.Project.Title,
                TaskStatus = c.TaskStatuses.OrderByDescending(s => s.Id).Select(v=>v.Status).FirstOrDefault()
                
            }).AsQueryable();

            if (dto.TaskId !=null)
            {
                query = query.Where(c => c.Id == dto.TaskId);
            }
            if (dto.ProjectId !=null)
            {
                query = query.Where(c => c.ProjectId == dto.ProjectId);
            }
            if (dto.Priority !=null)
            {
                query = query.Where(c => c.Priority == dto.Priority);
            }
            if (dto.DeadLine !=null)
            {
                query = query.Where(c => c.DeadLine <= dto.DeadLine);
            }
            if (dto.TaskStatus != null)
            {
                query = query.Where(c => c.TaskStatus == dto.TaskStatus);
            }

            if (dto.SortType==TaskSortType.Priority)
            {
                query = query.OrderBy(c => c.Priority);
            }
            else if (dto.SortType == TaskSortType.Deadline)
            {
                query = query.OrderBy(c => c.DeadLine);
            }
            else if (dto.SortType == TaskSortType.Id)
            {
                query = query.OrderBy(c => c.Id);
            }
            else if (dto.SortType == TaskSortType.Title)
            {
                query = query.OrderBy(c => c.Title);
            }
            else if (dto.SortType == TaskSortType.ProjectTitle)
            {
                query = query.OrderBy(c => c.ProjectTitle);
            }

            return await query.ToListAsync();

        }

        public async Task<List<TaskStatusResult>> GetTaskStatus(long taskId)
        {
            var taskStatusList = await _db.TaskStatuses
                                          .Where(c => c.ProjectTaskId == taskId)
                                          .Select(c => new TaskStatusResult
                                          {
                                              Status = c.Status,
                                              Time = c.Time
                                          }).ToListAsync();


            return taskStatusList;

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
