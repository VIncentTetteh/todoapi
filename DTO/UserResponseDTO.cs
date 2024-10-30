namespace TodoAPI.DTO
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
    }
}