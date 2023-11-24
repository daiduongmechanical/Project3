using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Data;
using Project3.Dtos;
using Twilio.TwiML.Voice;

namespace Project3.Component
{
    public class FriendViewComponent : ViewComponent
    {
        private readonly MyDbContext _context;

        public FriendViewComponent(MyDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(string name)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == name);

     var result = _context.Friends
    .Where(f => (f.SendId == user.Id && f.status!="Reject"))
    .Join(
        _context.Users,
        f => f.RecieveId,
        u => u.Id,
        (f, u) => new FriendDto
        {
            Avatar = u.Avatar,
            Name = u.UserName,
            id = u.Id,
            Description=u.Description,
            Status=f.status
           
            
        }
    )
    .Union(
        _context.Friends
            .Where(f => (f.RecieveId == user.Id && f.status != "Reject"))
            .Join(
                _context.Users,
                f => f.SendId,
                u => u.Id,
                (f, u) => new FriendDto
                {
                    Avatar = u.Avatar,
                    Name = u.UserName,
                    id = u.Id,
                    Description=u.Description,
                    Status=f.status

                }

            )
    )
    .ToList();
          
            return View(null, result);



        }
    }
}
