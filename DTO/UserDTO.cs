namespace TodoAPI.DTO
{
    public class UserDTO
    {
        public required string Email { get; set; }
        public required string Username { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public required string Password { get; set; }
    }
}