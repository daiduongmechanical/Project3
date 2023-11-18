using Project3.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project3.Models
{
    public class UserRole:BaseEntity
    {

        public int UserId {  get; set; }
        public int RoleId { get; set;}

        // Navigation properties using foreign keys
        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("RoleId")]
        public Role? Role { get; set; }
    }
}
