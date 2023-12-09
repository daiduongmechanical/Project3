using Project3.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project3.Dtos
{
    public class ContentDto
    {
        public int ServiceId { get; set; }

        public IFormFile Images { get; set; }

        public string? Title { get; set; }
        public string? Content { get; set; }
     
   

    }
}
