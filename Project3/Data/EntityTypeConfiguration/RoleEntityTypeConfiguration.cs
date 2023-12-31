﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project3.Models;

namespace Project3.Data.EntityTypeConfiguration
{
    public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(new List<Role>
            {
                new()
                {
                    Id = 1,
                    RoleName = "user"
                },
                new()
                {
                    Id = 2,
                    RoleName = "writer"
                },
                new()
                {
                    Id = 3,
                    RoleName = "admin"
                },
                new()
                {
                    Id = 4,
                    RoleName = "manager"
                }
            });
        }
    }
}