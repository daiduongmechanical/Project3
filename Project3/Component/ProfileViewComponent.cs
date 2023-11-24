using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Data;
using Project3.Dtos;
using Project3.Models;

namespace Project3.Component
{
    public class ProfileViewComponent :ViewComponent
    {
		private readonly MyDbContext _context;

		public ProfileViewComponent(MyDbContext context)
        {
            _context=context;
        }
		public IViewComponentResult Invoke(string name)
        {

            var user = _context.Users
                .Include(u=>u.Hobbies).ThenInclude(Hobbie=>Hobbie.TypeHobbie)
                .FirstOrDefault(u => u.UserName == name);

            var check= _context.TypeHobbies
                .ToList();
           
            return View(null, new ProfileDto { User=user,Hobbies=check});
        }
    }
}
