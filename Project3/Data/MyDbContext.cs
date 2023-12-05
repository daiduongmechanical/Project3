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
        public DbSet<Verified> verifieds { get; set; }
        public DbSet<TypeHobbie> TypeHobbies { get; set; }
        public DbSet<Hobbie> Hobbies { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<RoomMessage> RoomMessages { get; set; }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<RoomMember> RoomMembers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServicePrice> ServicePrice { get; set; }
        public DbSet<ServiceContent> ServiceContents { get; set; }
        public DbSet<ServiceRegistered> ServiceRegistereds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyDbContext).Assembly);
            modelBuilder.Entity<ServicePrice>(entity =>
            {
                entity.HasOne(x => x.Service)
                .WithMany(x => x.ServicePrices)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<ServiceContent>(entity =>
            {
                entity.HasOne(x => x.Service)
               .WithMany(x => x.ServiceContents)
               .HasForeignKey(x => x.ServiceId)
               .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<ServiceRegistered>(entity =>
            {
                entity.HasOne(x => x.ServicePrice)
               .WithMany()
               .HasForeignKey(x => x.Service_PriceId)
               .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.User)
               .WithMany(x => x.Registered)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}