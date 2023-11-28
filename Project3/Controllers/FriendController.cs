using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Data;
using Project3.Dtos;
using Project3.Models;
using Project3.Shared;
using System.Text.RegularExpressions;


namespace Project3.Controllers
{
    [AllowAnonymous]
    public class FriendController : BaseController
    {
        public FriendController(MyDbContext context) : base(context)
        {}
        [Microsoft.AspNetCore.Mvc.Route("/search")]
        public async Task<IActionResult> Index(string q)
        {
            var r = new Regex(@"^\d{8,12}$");
            ViewData["search"] = q;
            IQueryable<FriendDto> check;
            var id = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

            if (r.IsMatch(q))
            {
                check = from child in _context.Users.Where(u => u.Phone.Contains(q) &&  u.Id !=int.Parse(id))
                        join sender in _context.Friends on child.Id equals sender.SendId into s
                        from sender in s.DefaultIfEmpty()
                        join reciver in _context.Friends on child.Id equals reciver.RecieveId into re
                        from reciver in re.DefaultIfEmpty()
                        select new FriendDto
                        {
                            Name = child.UserName,
                            Avatar = child.Avatar,
                            Description = child.Description,
                            IsSender = child.Id == sender.SendId ? true : false,
                            id = child.Id,
                            Status = sender.status == null ? (reciver.status == null ? null : reciver.status) : sender.status
                        };
            }else{
                check = from child in _context.Users.Where(u => u.UserName.Contains(q) && u.Id != int.Parse(id))
                        join sender in _context.Friends on child.Id equals sender.SendId into s
                        from sender in s.DefaultIfEmpty()
                        join reciver in _context.Friends on child.Id equals reciver.RecieveId into re
                        from reciver in re.DefaultIfEmpty()
                        select new FriendDto
                        {
                            Name = child.UserName,
                            Avatar = child.Avatar,
                            Description=child.Description,
                            id = child.Id,
                            IsSender= child.Id==sender.SendId?true :false,
                            Status = sender.status == null ? (reciver.status == null ? null : reciver.status) : sender.status
                        };
            }
          await check.ToListAsync();
            return View(check);
        }

        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("/cancel/{sender}/{recieve}/{q?}/{name?}")]
        public async Task<IActionResult> CancelFriend(int sender, int recieve, string? q, string ? name)
        {
            var check = await _context.Friends
                 .FirstOrDefaultAsync(f => f.SendId == sender && f.RecieveId == recieve);
          
            
                _context.Friends.Remove(check);
                await _context.SaveChangesAsync();
            if (q == null)
            {
                return RedirectToAction("Index", "Profile", new { name = name});

            }
            else
            {
                return RedirectToAction("Index", "Friend", new { q = q });
            }
               
          
        
        }

        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("/accept/{sender}/{recieve}/{q?}/{name?}")]
        public async Task<IActionResult> AcceptFriend(int sender, int recieve, string? q, string? name)
        {
            var check = await _context.Friends.FirstOrDefaultAsync(f => f.SendId == sender && f.RecieveId == recieve);
            check.status = "Accept";
           await _context.SaveChangesAsync();
            if (q == null)
            {
                ViewData["success"] = "Accept request successfully";
                return RedirectToAction("Index", "Profile", new { name = name, text = "Require"});
            }
            else
            {
                ViewData["success"] = "Accept request successfully";
                return RedirectToAction("Index", "Friend", new {q=q });
            }


        }

        [HttpGet]
		[Microsoft.AspNetCore.Mvc.Route("/add/{sender}/{reciever}/{q?}")]
		public async Task<IActionResult> AddFriend(int sender, int reciever,  string? q)
        {
          _context.Friends.Add(new Models.Friend { RecieveId = reciever, SendId = sender });
         
            await _context.SaveChangesAsync();
            if (q==null)
            {
                var user = await _context.Users.FindAsync(reciever);
                TempData["success"] = "Add friend was send sucessfully";
                return RedirectToAction("Index", "Profile",new {name= user.UserName });
            }
            else
            {
                TempData["success"] = "Add friend was send sucessfully";
                return RedirectToAction("Index", new {q=q});
            }
        }



    }
}
