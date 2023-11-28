using Project3.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project3.Models
{
    public class RoomMember:BaseEntity
    {
        [ForeignKey("User")]
        public int MemberId { get; set; }
        [ForeignKey("Room")]
        public int RoomId { get; set; }
       
    }
}
