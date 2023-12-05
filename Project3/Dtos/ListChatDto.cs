using Project3.Models;

namespace Project3.Dtos
{
    public class ListChatDto
    {
        public int Id { get; set; }

        public string? name { get; set; }
        public string? avatar { get; set; }
        public Message? message { get; set; }
        public RoomMessage? roomMess { get; set; }
        public bool Isprivate { get; set; }

        public ListChatDto()
        {
            Isprivate = true;
        }
    }
}