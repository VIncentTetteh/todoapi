
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TodoAPI.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public required string Email { get; set; }
        public required string Username { get; set; }

        
        public string PhoneNumber { get; set; } = string.Empty;

        public required string Password { get; set; }

        public string OtpCode { get; set; } = string.Empty;
        public DateTime OtpExpiryTime { get; set; }
    }
}