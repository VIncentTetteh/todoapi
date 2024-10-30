
using Microsoft.AspNetCore.Mvc;
using TodoAPI.Data;

namespace TodoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ApplicationContext _context;
        public UserController(ApplicationContext context)
        {
            _context = context;
        }


    }
}