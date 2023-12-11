using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Data;
using Project3.Dtos;
using Project3.Hubs;
using Project3.Models;
using Project3.Shared;

namespace Project3.Controllers
{
    public class ChatController : BaseController
    {
        public ChatController(MyDbContext context) : base(context)
        {
        }

        [HttpGet]
        [Route("/chat/{type?}/{name?}")]
        public async Task<IActionResult> Index(string? type, string? name)
        {
            BaseMethod.ConvertTempData(TempData, ViewData, "success");
            BaseMethod.ConvertTempData(TempData, ViewData, "error");
            BaseMethod.ConvertTempData(TempData, ViewData, "joingroup");
            var componment = BaseText.Default;
            string data = "";
            var id = User.Claims.FirstOrDefault(c => c.Type == "id").Value;
            if (name != null && type != null)
            {
                componment = type == "private" ? "ChatPrivate" : "ChatGroup";
                data = name;
            }

            var ChatInfo = await _context.Messages
        .Where(m => m.ReceiverId == int.Parse(id) || m.SenderId == int.Parse(id))
        .Join(
            _context.Users,
            m => m.ReceiverId == int.Parse(id) ? m.SenderId : m.ReceiverId,
            u => u.Id,
            (message, user) => new { Message = message, User = user }
        )
        .GroupBy(result => result.User.Id)
        .Select(group => new ListChatDto
        {
            Id = group.Key,
            name = group.First().User.UserName,
            avatar = group.First().User.Avatar,
            message = group.Select(result => result.Message).OrderByDescending(m => m.CreatedDate).FirstOrDefault()
        })
        .ToListAsync();

            var ListGroup = await _context.RoomMembers.Where(m => m.MemberId == int.Parse(id) && m.IsMember)
                            .Join(_context.Rooms
                            .Include(r => r.Messages.OrderByDescending(msg => msg.CreatedDate).Take(1)),
                            m => m.RoomId,
                            r => r.Id,
                            (m, r) => new ListChatDto
                            {
                                Id = r.Id,
                                name = r.Name,
                                avatar = "room.png",
                                Isprivate = false,
                                roomMess = r.Messages.FirstOrDefault()
                            }

                            ).ToListAsync();

            ViewData["component"] = componment;
            ViewData["user"] = data;
            return View(ChatInfo.Union(ListGroup));
        }

        [HttpGet]
        [Route("/api/chat/up/{senId}/{reId}")]
        public async Task ChangeStatusMess(int senId, int reId)
        {
            var check = await _context.Messages
                 .Where(m => m.SenderId == senId && m.ReceiverId == reId && m.status == "Pending")
                 .ToListAsync();

            foreach (var i in check)
            {
                i.status = "Sent";
            }

            await _context.SaveChangesAsync();
        }

        //add group

        [HttpPost]
        public async Task<IActionResult> AddGroup(string name, string userid)
        {
            var room = new Room { Name = name };
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();

            await _context.RoomMembers.AddAsync(new RoomMember { Role = "Admin", MemberId = int.Parse(userid), RoomId = room.Id });
            await _context.SaveChangesAsync();
            TempData["success"] = "Create room successfully";
            TempData["joingroup"] = $"{room.Name}_s{room.Id}";
            return RedirectToAction("Index", new { type = "room", name = name + $"_s{room.Id}" });
        }

        public async Task<IActionResult> AddToGroup(List<int>? users, string groupId)
        {
            if (users != null)
            {
                var id = int.Parse(groupId.Split("_s")[1]);
                var current = await _context.RoomMembers.Where(r => r.RoomId == id)
                    .ToListAsync();
                foreach (var user in users)
                {
                    var check = current.FirstOrDefault(c => c.MemberId == user);
                    if (check == null)
                    {
                        _context.RoomMembers.Add(new RoomMember { MemberId = user, RoomId = int.Parse(groupId.Split("_s")[1]) });
                    }
                    else
                    {
                        check.IsMember = true;
                        _context.RoomMembers.Update(check);
                    }
                }
            }
            await _context.SaveChangesAsync();
            TempData["success"] = "add to group successfully";
            return RedirectToAction("Index", new { type = "group", name = groupId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserGroup(List<int> mem, string roomId)
        {
            var id = roomId.Split("_s")[1];
            if (mem != null)
            {
                foreach (var i in mem)
                {
                    var member = _context.RoomMembers.FirstOrDefault(r => r.MemberId == i && r.RoomId == int.Parse(id));
                    member.IsMember = false;
                }
                await _context.SaveChangesAsync();
                TempData["success"] = "Remove user rom group successully";
                return RedirectToAction("index", new { type = "group", name = roomId });
            }
            TempData["error"] = "Have error please try again";
            return RedirectToAction("index", new { type = "group", name = roomId });
        }

        [HttpGet]
        [Route("/ChangeRole/{roomId}/{userId}/{role}")]
        public async Task<IActionResult> ChangeRole(string roomId, int userId, string role)
        {
            var id = roomId.Split("_s")[1];
            var data = await _context.RoomMembers.FirstOrDefaultAsync(m => m.MemberId == userId && m.RoomId == int.Parse(id));
            data!.Role = BaseMethod.UpperFirst(role);
            await _context.SaveChangesAsync();
            TempData["success"] = "Update role for user successfully";
            return RedirectToAction("index", new { type = "group", name = roomId });
        }

        [HttpGet]
        [Route("/leavegroup/{userId}/{groupId}")]
        public async Task<IActionResult> LeaveGroup(int userId, int groupId)
        {
            var check = await _context.RoomMembers.FirstOrDefaultAsync(r => r.RoomId == groupId && r.MemberId == userId);
            check.IsMember = false;
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }
    }
}