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
        public IActionResult Index(string name)
        {
            var GroupName = name.Split("_s")[0];
            var GroupId = name.Split("_s")[1];

            var data = _context.Rooms
                .Include(m => m.members)
                .Include(r => r.Messages).ThenInclude(m => m.User)
                .Where(r => r.Id == int.Parse(GroupId))
                .ToList();

            return Ok(data);
        }
    }
}