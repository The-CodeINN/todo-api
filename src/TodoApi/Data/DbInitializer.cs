using Microsoft.EntityFrameworkCore;
using TodoApi.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TodoApi.Data
{
    public class DbInitializer
    {
        public static void InitDb(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TodoContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbInitializer>>();
            SeedData(context, logger);
        }

        private static void SeedData(TodoContext context, ILogger<DbInitializer> logger)
        {
            context.Database.Migrate();

            if (context.TodoItems.Any())
            {
                logger.LogInformation("Already have data - no need to seed!");
                return;
            }

            var todoItems = new List<TodoItem>
            {
                new TodoItem { Title = "Learn C#", IsCompleted = false },
                new TodoItem { Title = "Build a .NET API", IsCompleted = false },
                new TodoItem { Title = "Write an article about .NET", IsCompleted = false },
                new TodoItem { Title = "Create a Todo app", IsCompleted = true },
                new TodoItem { Title = "Understand CQRS pattern", IsCompleted = false }
            };

            context.AddRange(todoItems);
            context.SaveChanges();

            logger.LogInformation("Data seeded successfully!");
        }
    }
}
