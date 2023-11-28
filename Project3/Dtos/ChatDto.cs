using Project3.Models;

namespace Project3.Dtos
{
    public class ChatDto
    {
        public User User { get; set; }
        public ICollection<Message>? Messages { get; set; }

    }
}
