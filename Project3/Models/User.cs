using Project3.Dtos;
using Project3.Shared;
using System.ComponentModel.DataAnnotations;

namespace Project3.Models
{
    public class User : BaseEntity
    {
        public string? UserName { get; set; }
        [Required]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*#?&])[A-Za-z\\d@$!%*#?&]{8,}$")]
        public string? Password { get; set; }
        [EmailAddress]
        public string? Email { get; set; }

        public string? Description { get; set; }
        public string? Avatar { get; set; }

        public DateTime Dob { get; set; }
        public string? Address { get; set; }

        public bool Verified { get; set; }
        [Required]
        [RegularExpression("/\\(?([0 - 9]{3})\\)? ([ .-]?) ([0 - 9]{3})\\2([0 - 9]{ 4})/")]

        public string? Phone { get; set; }
        public ICollection<UserRole>? Roles { get; }
        public ICollection<Device>? Devices { get; }
        public ICollection<Friend> Friends{ get; }

        public ICollection<Message> Messages { get; set; }

        public ICollection<RoomMember> Rooms { get; }
        public virtual ICollection<ServiceRegistered> Registered { get; set; } = new List<ServiceRegistered>();
        public List<Hobbie> Hobbies { get; }
        public User(RegisterDto data) {
            UserName = data.Name;
            Password= BCrypt.Net.BCrypt.HashPassword(data.Pass);
            Phone = $"{data.Head}{data.Phone}";
            Verified = false;
            Avatar = "default.jpg";
        }
        public User() { }
        

        }
    
}
