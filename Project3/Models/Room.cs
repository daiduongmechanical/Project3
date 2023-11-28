using Project3.Shared;

namespace Project3.Models
{
    public class Room:BaseEntity
    {
        public string Name { get; set; }
        public List<RoomMember>? Members { get;}
    }
}
