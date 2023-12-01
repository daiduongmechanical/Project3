using Project3.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project3.Models
{
    public class ServiceRegistered : BaseEntity
    {
        public int Service_Price_Id { get; set; }
        public string Status { get; set; }
        public virtual ServicePrice ServicePrice { get; set; }
        public int User_Id { get; set; }
        public virtual User User { get; set; }

        public ServiceRegistered()
        {
            Status = "Pending";
        }
    }
}
