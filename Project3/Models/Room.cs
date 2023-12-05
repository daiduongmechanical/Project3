using Project3.Shared;

namespace Project3.Models
{
    public class Room : BaseEntity
    {
        public string? Name { get; set; }
        public ICollection<RoomMessage>? Messages { get; }
        public ICollection<RoomMember>? members { get; }
    }
}