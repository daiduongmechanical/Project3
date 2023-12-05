using Project3.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project3.Models
{
    public class Service : BaseEntity
    {
        public string Name { get; set; } = null!;

        public virtual ICollection<ServicePrice>? ServicePrices { get; set; }
        public virtual ICollection<ServiceContent>? ServiceContents { get; set; }
    }
}