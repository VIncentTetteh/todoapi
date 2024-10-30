namespace TodoAPI.DTO
{
    public class TodoDTO
    {

        public required string Title { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
    }
}