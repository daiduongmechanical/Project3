using Project3.Shared;

namespace Project3.Models
{
    public class Verified:BaseEntity
    {
        public string? Code { get; set; }
        public int UserId { get; set; }
        public bool IsLife {  get; set; }
        public Verified() { 
        IsLife = true;
            Code = Guid.NewGuid().ToString().Substring(0,6);
        }
    }

}
