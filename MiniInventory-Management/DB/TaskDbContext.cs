using Microsoft.EntityFrameworkCore;
using MiniInventory_Management.Model;

namespace MiniInventory_Management.DB
{
    public class TaskDbContext:DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }

    }
}
