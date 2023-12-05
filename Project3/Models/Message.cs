using Project3.Shared;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Project3.Models
{
    public class Message : BaseEntity
    {
        public string? Content { get; set; }

        public int SenderId { get; set; }
        public int? ReceiverId { get; set; }
        public string? status { get; set; }

        public Message()
        {
            status = "Pending";
        }
    }
}