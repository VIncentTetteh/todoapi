using System.ComponentModel.DataAnnotations;
using TodoAPI.Models;

namespace TodoAPI.Models
{
    public class Todo
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public required string Title { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public required Guid UserId { get; set; }
 
        public User User { get; set; } = null!;
    }
}