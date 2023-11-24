using Project3.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project3.Models
{
    public class Friend:BaseEntity
    {
        [ForeignKey("User")]
      public  int SendId { get; set; }
		[ForeignKey("User")]
		public int RecieveId { get; set; }
		[RegularExpression("Acceppt|Pending|Reject", ErrorMessage = "Invalid Status")]
		public string? status { get; set; }
    
        public Friend(){
            status = "Pending";

		}
	}

}
