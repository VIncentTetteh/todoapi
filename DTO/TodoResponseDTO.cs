namespace TodoAPI.DTO
{
    public class TodoResponseDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public Guid UserId { get; set; }
        public UserResponseDTO User { get; set; }
    }
}