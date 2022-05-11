using Microsoft.EntityFrameworkCore;

namespace Trackable_API.Models
{
    public class TaskContext : DbContext
    {

        public TaskContext(DbContextOptions<TaskContext> options)
            : base(options)
        {

        }

        public DbSet<Task> Tasks { get; set; } = null!;
    }
}
