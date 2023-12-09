using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project3.Models;

namespace Project3.Data.EntityTypeConfiguration
{
    public class RoomMessageEntityTypeConfiguration : IEntityTypeConfiguration<RoomMessage>
    {
        public void Configure(EntityTypeBuilder<RoomMessage> builder)
        {
            builder.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);
        }
    }
}