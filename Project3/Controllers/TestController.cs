using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Data;
using Project3.Shared;

namespace Project3.Controllers
{
    public class TestController : BaseController

    {
        public TestController(MyDbContext context) : base(context)
        {
        }

        [Route("/test/chat/{name}")]
        public async Task<IActionResult> Index(string name)
        {
            var id = name.Split("_s")[1];
            var data = await _context.RoomMessages.
                Include(m => m.User).
                   Where(m => m.RoomId == int.Parse(id))
                   .OrderByDescending(m => m.CreatedDate).FirstAsync();

            return Ok(data);
        }
    }
}