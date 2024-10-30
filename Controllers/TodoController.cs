using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Data;
using TodoAPI.DTO;
using TodoAPI.Models;

namespace TodoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TodoController: ControllerBase{
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;
    
    public TodoController(ApplicationContext context, IMapper mapper){
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodo(TodoDTO todoDto){

        #pragma warning disable CS8604 // Possible null reference argument.
        var userIdString = User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
        #pragma warning restore CS8604 // Possible null reference argument.

        if (!Guid.TryParse(userIdString, out var userId))
        {
            return BadRequest("Invalid user ID format.");
        }

        var todo = new Todo
        {
            Title = todoDto.Title,
            Description = todoDto.Description,
            UserId = userId,
            IsCompleted = todoDto.IsCompleted
        };

        var todoTitleExistsForUser = await _context.Todos.AnyAsync(t => t.Title == todo.Title && t.UserId == userId);
        if (todoTitleExistsForUser)
            return BadRequest(new { message = "Todo with this title already exists" });

        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();  
        return Ok(todo);

        
    }

    [HttpGet]
    public async Task<IActionResult> GetUserTodos()
    {
        #pragma warning disable CS8604 // Possible null reference argument.
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        #pragma warning restore CS8604 // Possible null reference argument.
        var todos = await _context.Todos
        .Include(t => t.User)
        .Where(t => t.UserId == userId)
        .ToListAsync();

        // Map to List<TodoResponseDto>
        var todosResponse = _mapper.Map<List<TodoResponseDTO>>(todos);
        
        return Ok(todosResponse);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, TodoDTO todoDto)
    {
        
        var todo = await _context.Todos
        .Include(t => t.User)
        .Where(t => t.Id == id)
        .FirstOrDefaultAsync();

        if (todo.UserId != Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))) return Unauthorized();

        if (todo == null) return NotFound();

        todo.Description = todoDto.Description;
        todo.IsCompleted = todoDto.IsCompleted;

        await _context.SaveChangesAsync();

        // Map to List<TodoResponseDto>
        var todoResponse = _mapper.Map<TodoResponseDTO>(todo);
        return Ok(todoResponse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var todo = await _context.Todos.FindAsync(id);

        #pragma warning disable CS8604 // Possible null reference argument.
        #pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (todo.UserId != Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))) return Unauthorized();
        #pragma warning restore CS8602 // Dereference of a possibly null reference.
        #pragma warning restore CS8604 // Possible null reference argument.

            if (todo == null) return NotFound();

        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Todo deleted" });
    }

    }

    
}