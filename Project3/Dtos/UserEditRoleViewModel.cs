using Project3.Models;

namespace Project3.Dtos
{
    public class UserEditRoleViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<Role> UserRoles { get; set; }
        public List<Role> AllRoles { get; set; }
        public List<int> SelectedRoleIds { get; set; }
    }
}
