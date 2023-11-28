using Microsoft.AspNetCore.Mvc;
using Project3.Data;
using Project3.Dtos;

namespace Project3.Component
{
	public class RequireViewComponent : ViewComponent
	{
		private readonly MyDbContext _context;

		public RequireViewComponent(MyDbContext context) { 
		_context=context;
		}
		public IViewComponentResult Invoke(string name)
		{
			var user = _context.Users.FirstOrDefault(u => u.UserName == name);
			var result = _context.Friends
		   .Where(f => (f.SendId == user.Id) && f.status!="Accept")
		   .Join(
			   _context.Users,
			   f => f.RecieveId,
			   u => u.Id,
			   (f, u) => new FriendDto
			   {
				   Avatar = u.Avatar,
				   Name = u.UserName,
				   id = u.Id,
				   Description = u.Description,
				   Status = f.status,
				   IsSender = true
			   }
		   )
		   .Union(
			   _context.Friends
				   .Where(f => (f.RecieveId == user.Id && f.status != "Accept"))
				   .Join(
					   _context.Users,
					   f => f.SendId,
					   u => u.Id,
					   (f, u) => new FriendDto
					   {
						   Avatar = u.Avatar,
						   Name = u.UserName,
						   id = u.Id,
						   Description = u.Description,
						   Status = f.status,
						   IsSender = false
					   }
				   )
		   )
		   .ToList();

			return View(null, result);
		}
	}
}
