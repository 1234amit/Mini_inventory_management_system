using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace MiniInventory_Management.DB
{     
    public class TaskDbContextFactory : IDesignTimeDbContextFactory<TaskDbContext>
        {
            public TaskDbContext CreateDbContext(string[] args)
            {
                // Read configuration from appsettings.json
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory()) // current path of the project
                    .AddJsonFile("appsettings.json")
                    .Build();

                var optionsBuilder = new DbContextOptionsBuilder<TaskDbContext>();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

                return new TaskDbContext(optionsBuilder.Options);
            }
        }
    
}
