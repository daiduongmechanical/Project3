using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project3.Models;

namespace Project3.Data.EntityTypeConfiguration
{
	public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.HasMany(user => user.Messages)
				.WithOne()
				.HasForeignKey(message => message.SenderId);
			builder.HasData(new List<User>{
				new()
				{
					UserName="xhaka",
					Password=BCrypt.Net.BCrypt.HashPassword("Hello123@"),
					Phone="+84234456678",
					Id=5,
						Verified = true,
			Avatar = "default.jpg",
			IsBlocked = false,
			Email="vainhoo@gmail.com"
		},new()
				{
					UserName="saka",
					Password=BCrypt.Net.BCrypt.HashPassword("Hello123@"),
					Phone="+8477885566",
					Id=6,
						Verified = true,
			Avatar = "default.jpg",
			IsBlocked = false,
			Email="vainhoo@gmail.com"
		},
new()
				{
					UserName="rose",
					Password=BCrypt.Net.BCrypt.HashPassword("Hello123@"),
					Phone="+84987765543",
					Id=7,
						Verified = true,
			Avatar = "default.jpg",
			IsBlocked = false,
			Email="vainhoo@gmail.com"
		},
new()
				{
					UserName="atetar",
					Password=BCrypt.Net.BCrypt.HashPassword("Hello123@"),
					Phone="+8422665544",
					Id=8,
						Verified = true,
			Avatar = "default.jpg",
			IsBlocked = false,
			Email="vainhoo@gmail.com"
		},
new()
				{
					UserName="enketia",
					Password=BCrypt.Net.BCrypt.HashPassword("Hello123@"),
					Phone="+84987865454",
					Id=9,
						Verified = true,
			Avatar = "default.jpg",
			IsBlocked = false,
			Email="vainhoo@gmail.com"
		},
			}

			);
		}
	}
}