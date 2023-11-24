using Project3.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project3.Models
{
    public class UserRole:BaseEntity
    {
        [ForeignKey("User")]
        public int UserId {  get; set; }
    
        public int RoleId { get; set;}
        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        // Navigation properties using foreign keys
     
  
   
    }
}
