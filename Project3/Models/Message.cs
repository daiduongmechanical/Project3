using Project3.Shared;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Project3.Models
{
    public class Message:BaseEntity
    {
 
        public string? Content { get; set; }

        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        [NotMapped]
        [ForeignKey("SenderId")]
        public virtual User? Sender { get; set; }
        [NotMapped]
        [ForeignKey("ReceiverId")]
        public virtual User? Receiver { get; set; }
    }
}
