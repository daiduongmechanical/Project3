using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Project3.Data;
using Project3.Models;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace Project3.Hubs
{
    public class OnlineStatus
    {
        public string? Name { get; set; }
        public bool IsOnline { get; set; }
    }

    public class ChatHub : Hub
    {
        private readonly MyDbContext _context;
        private static ConcurrentDictionary<string, string> userConnections = new ConcurrentDictionary<string, string>();

        public ChatHub(MyDbContext context)
        {
            _context = context;
        }

        public async Task JoinGroup(string GroupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GroupId);
        }

        public async Task SendMessageToGroup(string GroupId, string user, string message)
        {
            var RoomMess = new RoomMessage()
            {
                Content = message,
                UserId = int.Parse(user),
                RoomId = int.Parse(GroupId.Split("_s")[1])
            };
            _context.RoomMessages.Add(RoomMess);
            await _context.SaveChangesAsync();
            await Clients.Group(GroupId).SendAsync("ReceiveMessageGroup", user, message);
        }

        public async Task CheckOnline(List<string> ListCheck)
        {
            var Result = new List<OnlineStatus>();
            foreach (var u in ListCheck)
            {
                if (userConnections.TryGetValue(u, out var connectionId))
                {
                    Result.Add(new OnlineStatus { IsOnline = true, Name = u });
                }
                else
                {
                    Result.Add(new OnlineStatus { IsOnline = false, Name = u });
                }
            }

            await Clients.Caller.SendAsync("CheckOnlineResult", Result);
        }

        public async Task NotifyMessage(string UserName, string userID)
        {
            var check = await _context.Messages
     .Where(m => m.ReceiverId == int.Parse(userID) && m.status == "Pending")
     .Join(
         _context.Users,
         message => message.SenderId,
         user => user.Id,
         (message, user) => new { Message = message, User = user }
     )
     .GroupBy(x => x.Message.SenderId)
     .ToListAsync();
            if (userConnections.TryGetValue(UserName, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("NotifyNewMessage", check);
            }
        }

        // send messgae
        public async Task SendMessage(string toUser, string message, string reciveId)
        {
            try
            {
                //create instance message
                var id = Context.User.Claims.FirstOrDefault(c => c.Type == "id").Value;
                var newMess = new Models.Message()
                {
                    Content = message,
                    ReceiverId = int.Parse(reciveId),
                    SenderId = int.Parse(id),
                };

                //check friend
                var checkFriend = await _context.Friends
                    .FirstOrDefaultAsync(f => f.SendId == int.Parse(id) && f.RecieveId == int.Parse(reciveId)
                    || f.SendId == int.Parse(reciveId) && f.RecieveId == int.Parse(id));
                if (checkFriend == null || checkFriend.status == "Pending")
                {
                    var count = await _context.Messages.CountAsync(m => m.SenderId == int.Parse(id) && m.ReceiverId == int.Parse(reciveId));
                    if (count > 4)
                    {
                        throw new Exception("FriendError");
                    }
                }
                if (userConnections.TryGetValue(toUser, out var connectionId))
                {
                    await _context.AddAsync(newMess);
                    await _context.SaveChangesAsync();
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", Context.User.FindFirstValue(ClaimTypes.NameIdentifier), message);

                    var check = await _context.Messages
                       .Where(m => m.ReceiverId == int.Parse(reciveId) && m.status == "Pending")
                       .Join(
                                _context.Users,
                                 message => message.SenderId,
                                 user => user.Id,
          (message, user) => new { Message = message, User = user }
      )
      .GroupBy(x => x.Message.SenderId)
      .ToListAsync();
                    await Clients.Client(connectionId).SendAsync("NotifyNewMessage", check);
                }
                else
                {
                    await _context.AddAsync(newMess);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in SendMessage: {ex.Message}");
                throw;
            }
        }

        public override Task OnConnectedAsync()
        {
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            userConnections.TryAdd(userId, Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            userConnections.TryRemove(userId, out _);
            return base.OnDisconnectedAsync(exception);
        }
    }
}