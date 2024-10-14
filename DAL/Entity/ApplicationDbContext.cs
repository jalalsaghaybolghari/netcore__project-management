using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the ApplicationDbContext class with the specified DbContext options.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // The base constructor handles initializing the DbContext with the provided options.
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<ProjectRole> ProjectRoles { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<TaskMember> TaskMembers { get; set; }
        public DbSet<TaskStatus> TaskStatuses { get; set; }





    }
}
