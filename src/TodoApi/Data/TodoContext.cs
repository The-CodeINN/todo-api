using Microsoft.EntityFrameworkCore;
using TodoApi.Entities;

namespace TodoApi.Data;

public class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options)
{

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>()
            .Property(t => t.Id)
            .ValueGeneratedOnAdd();
    }

    public DbSet<TodoItem> TodoItems { get; set; }
}
