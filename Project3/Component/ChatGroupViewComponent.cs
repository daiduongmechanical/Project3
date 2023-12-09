using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayPal.Api;
using Project3.Data;
using Project3.Models;

namespace Project3.Component
{
    public class ChatGroupViewComponent : ViewComponent
    {
        private readonly MyDbContext _context;
        private IHttpContextAccessor _httpContextAccessor;

        public ChatGroupViewComponent(MyDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IViewComponentResult Invoke(string name)
        {
            var GroupName = name.Split("_s")[0];
            var GroupId = name.Split("_s")[1];
            var id = HttpContext.User.FindFirst("id").Value;

            var data = _context.Rooms
                .Include(m => m.members.Where(m => m.IsMember)).ThenInclude(m => m.user)
                .Include(r => r.Messages).ThenInclude(m => m.User)
                .FirstOrDefault(r => r.Id == int.Parse(GroupId));

            if (data.members.Select(r => r.MemberId).Contains(int.Parse(id)))
            {
                return View(null, data);
            }
            else
            {
                _httpContextAccessor.HttpContext.Response.Redirect("/chat");
                return View();
            }
        }
    }
}