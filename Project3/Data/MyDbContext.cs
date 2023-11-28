using Microsoft.EntityFrameworkCore;
using Project3.Models;

namespace Project3.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Verified> verifieds {get;set;}
        public DbSet<TypeHobbie> TypeHobbies { get; set; }
        public DbSet<Hobbie> Hobbies { get; set; }
		public DbSet<Friend> Friends { get; set; }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<RoomMember> RoomMembers { get; set; }


        

    }
}
