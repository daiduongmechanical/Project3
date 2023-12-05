using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Data;
using Project3.Models;

namespace Project3.Component
{
    public class ChatGroupViewComponent : ViewComponent
    {
        private readonly MyDbContext _context;

        public ChatGroupViewComponent(MyDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(string name)
        {
            var GroupName = name.Split("_s")[0];
            var GroupId = name.Split("_s")[1];

            var data = _context.Rooms
                .Include(m => m.members)
                .Include(r => r.Messages).ThenInclude(m => m.User)
                .FirstOrDefault(r => r.Id == int.Parse(GroupId));

            return View(null, data);
        }
    }
}