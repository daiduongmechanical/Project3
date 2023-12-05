using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Data;
using Project3.Dtos;

namespace Project3.Component
{
    public class ChatPrivateViewComponent: ViewComponent
    {
        private readonly MyDbContext _context;
        public  ChatPrivateViewComponent(MyDbContext context) { 
            _context = context;
        }

        public IViewComponentResult Invoke(string name)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == name);

            var id = HttpContext.User.FindFirst("id").Value;

           var PendingMess= _context.Messages
                .Where(m => m.ReceiverId == int.Parse(id) && m.SenderId == user.Id && m.status == "Pending")
                .ToList();

            foreach(var i in PendingMess)
            {
                i.status = "Sent";
            }
            _context.SaveChanges();

            var message =  _context.Messages
                .Where(m => m.SenderId == int.Parse(id) && m.ReceiverId == user.Id || m.SenderId == user.Id                 
                && m.ReceiverId == int.Parse(id)).ToList();
            return View(null,new ChatDto {User=user,Messages=message });
        }
    }
}
