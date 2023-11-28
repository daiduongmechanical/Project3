
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Project3.Data;
using Project3.Models;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace Project3.Hubs
{
    public class ChatHub : Hub
    {
        private readonly MyDbContext _context;
        private static ConcurrentDictionary<string, string> userConnections = new ConcurrentDictionary<string, string>();

        public ChatHub(MyDbContext context)
        {
            _context = context;
        }
        // send messgae


        public async Task SendMessage(string toUser, string message, string reciveId)
        {

            if (userConnections.TryGetValue(toUser, out var connectionId))
            {
                var id = Context.User.Claims.FirstOrDefault(c => c.Type == "id").Value;
                var newMess = new Message()
                {
                    Content = message,
                    ReceiverId = int.Parse(reciveId),
                    SenderId = int.Parse(id)
                };
                await _context.AddAsync(newMess);
                await _context.SaveChangesAsync();
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", Context.User.FindFirstValue(ClaimTypes.NameIdentifier), message);
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
