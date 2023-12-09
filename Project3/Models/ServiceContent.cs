using Project3.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project3.Models
{
    public class ServiceContent : BaseEntity
    {
        public int ServiceId { get; set; }
        public virtual Service? Service { get; set; }
        public string Title { get; set; } 


        [DataType(DataType.Text)]
        public string Content { get; set; } = null!;

        public string? image { get; set; }
    }
}