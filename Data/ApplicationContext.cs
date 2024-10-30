
using Microsoft.EntityFrameworkCore;
using TodoAPI.Models;

namespace TodoAPI.Data
{
    public class ApplicationContext : DbContext
    {

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) {}
        
        public DbSet<Todo> Todos { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => new {u.Email, u.PhoneNumber})
                .IsUnique();

            builder.Entity<Todo>()
                .HasIndex(t => t.Title)
                .IsUnique();
        }
    }
    
}