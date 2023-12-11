using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Data;
using Project3.Dtos;
using Project3.Shared;

namespace Project3.Controllers
{
    public class TestController : BaseController

    {
        public TestController(MyDbContext context) : base(context)
        {
        }

        [Route("test/{id}/{room}")]
        public async Task<IActionResult> GetListFriend(int id, string room)
        {
            return Ok();
        }
    }
}