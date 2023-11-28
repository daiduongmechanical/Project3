using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Data;
using Project3.Dtos;
using Project3.Shared;

namespace Project3.Controllers
{
    public class ChatController : BaseController
    {
       
        public ChatController(MyDbContext context) : base(context){}

        [HttpGet]
        [Route("/chat/{name}")]
        public async Task<IActionResult> Index( string name)
        {
            var user=await _context.Users.FirstOrDefaultAsync(u=>u.UserName==name);
            var id = User.Claims.FirstOrDefault(c => c.Type == "id").Value;
            var message =await _context.Messages
                .Where(m => m.SenderId == int.Parse(id) && m.ReceiverId == user.Id || m.SenderId== user.Id && m.ReceiverId== int.Parse(id)).ToListAsync();
            return View(new ChatDto { Messages=message, User=user});
        }
    }
}
