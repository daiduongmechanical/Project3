using Project3.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project3.Models
{
    public class RoomMessage : BaseEntity
    {
        public string? Content { get; set; }
        public int? UserId { get; set; }

        public int? RoomId { get; set; }

        public User? User { get; set; }
    }
}