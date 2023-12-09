using Project3.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project3.Models
{
    public class RoomMember : BaseEntity
    {
        public int? MemberId { get; set; }

        [ForeignKey("Room")]
        public int? RoomId { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsMember { get; set; }

        public User? user { get; set; }

        public RoomMember()
        {
            IsAdmin = false;
            IsMember = true;
        }
    }
}