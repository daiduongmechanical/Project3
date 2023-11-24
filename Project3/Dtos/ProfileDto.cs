using Project3.Models;

namespace Project3.Dtos
{
    public class ProfileDto
    {
        public User User { get; set; }
        public List<TypeHobbie> Hobbies { get; set; }
    }
}
