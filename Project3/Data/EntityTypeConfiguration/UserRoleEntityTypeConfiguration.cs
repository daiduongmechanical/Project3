using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project3.Models;

namespace Project3.Data.EntityTypeConfiguration
{
    public class UserRoleEntityTypeConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasData(
            new List<UserRole>
            {
                new()
                {
                    Id=1,
                    UserId=1,
                    RoleId=1
                },
                 new()
                {
                    Id=2,
                    UserId=2,
                    RoleId=1
                },
                  new()
                {
                    Id=3,
                    UserId=3,
                    RoleId=1
                },
                   new()
                {
                    Id=4,
                    UserId=4,
                    RoleId=1
                },
                    new()
                {
                    Id=5,
                    UserId=5,
                    RoleId=1
                },

                 new()
                {
                    Id=6,
                    UserId=2,
                    RoleId=4
                },
                  new()
                {
                    Id=7,
                    UserId=3,
                    RoleId=3
                },
            });
        }
    }
}