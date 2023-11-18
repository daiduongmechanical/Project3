using Project3.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project3.Models
{
    public class Device:BaseEntity
    {
        public string? DeviceId { get; set; }
        public string? DeviceName { get; set; }
        public string? DeviceType { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
    }
}
