using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project3.Models;

namespace Project3.Data.EntityTypeConfiguration
{
    public class RoomMemberEntityTypeConfiguration : IEntityTypeConfiguration<RoomMember>
    {
        public void Configure(EntityTypeBuilder<RoomMember> builder)
        {
            builder.HasOne(r => r.user)
                .WithMany()
                .HasForeignKey(r => r.MemberId);
        }
    }
}