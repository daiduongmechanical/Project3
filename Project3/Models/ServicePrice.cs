using Project3.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project3.Models
{
    public class ServicePrice : BaseEntity
    {
        public int Price { get; set; }
        public int Duration { get; set; }
        public int Service_Id { get; set; } 
        public virtual Service Service { get; set; }
        public virtual ICollection<ServiceRegistered> Registered { get; set; } = new List<ServiceRegistered>();
    }
}
