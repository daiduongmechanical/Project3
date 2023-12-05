using Project3.Shared;

namespace Project3.Models
{
    public class ServicePrice : BaseEntity
    {
        public int Price { get; set; }
        public int Duration { get; set; }
        public int ServiceId { get; set; }
        public virtual Service? Service { get; set; }
    }
}