using System.ComponentModel.DataAnnotations;

namespace Project3.Shared
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedDate { get; protected set; }
        public DateTime UpdatedDate { get; set; }

        public BaseEntity()
        {
            UpdatedDate = DateTime.Now;
            CreatedDate = DateTime.Now;
        }
    }
}