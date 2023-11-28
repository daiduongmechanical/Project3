using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Data;
using Project3.Dtos;
using Project3.Models;
using Project3.Shared;

namespace Project3.Controllers
{
	[AllowAnonymous]
	public class ProfileController : BaseController
	{
		public ProfileController(MyDbContext context) : base(context)
		{
		}
		[HttpGet]
		[Route("profile/{name}/{text?}")]
		public async Task<IActionResult> Index(string name, string? text)
		{
			BaseMethod.ConvertTempData(TempData, ViewData, "error");
			BaseMethod.ConvertTempData(TempData, ViewData, "success");
			string component = "Profile";
			
			if (text != null)
			{
				component = text.First().ToString().ToUpper() + text.Substring(1);

			}
			var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == name);
			var data = new FriendDto{
				Avatar=user.Avatar,
				id=user.Id,
				Description=user.Description,
				Status=null,
				IsSender=false,
				Name=user.UserName
			};
			var LoginId =  User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			var check=  await _context.Friends.FirstOrDefaultAsync(f => f.SendId == user.Id && f.RecieveId == int.Parse(LoginId)
			|| f.SendId == int.Parse(LoginId) && f.RecieveId == user.Id);
			if (check != null) {
				data.IsSender= check.SendId==user.Id?true:false;
				data.Status = check.status;
			}


            ViewData["view"] = component;
			return View(null, data);
		}
		[HttpPost]
		public async Task<IActionResult> UpdateInfo(string id, string phone, string mail, DateTime dob, string address)
		{

			var user = await _context.Users.FindAsync(int.Parse(id));
			if (id != User.FindFirst("id").Value)
			{
				TempData["error"] = "Update not legal,";
				return RedirectToAction("Index", new { name = user.UserName });
			}
			user.Email = mail;
			user.Address = address;
			user.Phone = phone;
			user.Dob = dob;

			await _context.SaveChangesAsync();
			TempData["success"] = "Update data successfully";
			return RedirectToAction("Index", new { name = user.UserName });
		}

		[HttpPost]
		public async Task<IActionResult> UpdateHobbies(string id, int[]? type, string[]? detail)
		{
			var user = _context.Users.FirstOrDefault(u => u.Id == int.Parse(id));
			if (type == null)
			{
				return RedirectToAction("Index", new {name=user.UserName} );
			}
			var hobbies = await _context.Hobbies.Where(h => h.UserId == int.Parse(id)).ToListAsync();

			//check to update and create
			for (var i = 0; i < type.Count(); i++)
			{
				var check = hobbies.FirstOrDefault(h => h.TypeId == type[i]);
				if (check == null)
				{
					_context.Hobbies.Add(new Models.Hobbie
					{
						detai = detail[i],
						UserId = int.Parse(id),
						TypeId = type[i]
					});
				}
				else
				{
					check.detai = detail[i];
				}

			}
			//check to delete
			foreach (var i in hobbies)
			{
				if (!type.Contains(i.TypeId))
				{
					_context.Remove(i);
				}
			}
			TempData["success"] = "update successfully";
			await _context.SaveChangesAsync();
			return RedirectToAction("Index", new { name = user.UserName });
		}

		public async Task<IActionResult> UpdateImage(IFormFile? img, string? description, string name, string id)
		{
			var user = await _context.Users.FindAsync(int.Parse(id));
			if (user.UserName != name)
			{
				var checkName = await _context.Users.FirstOrDefaultAsync(u => u.UserName == name);
				if (checkName != null)
				{
					TempData["error"] = "this name is alreadly exist , please select other names";
					return RedirectToAction("Index", new { name = user.UserName });
				}
			}

			if (img != null)
			{
				var newImage = await BaseMethod.UploadImage(img);
				if (newImage.Result)
				{
					BaseMethod.DeleteFile(user.Avatar);
					user.Avatar = newImage.Text;
				}
				else
				{
					TempData["error"] = "have error when upload image , please try later";
					return RedirectToAction("Index", new { name = user.UserName });
				}
			}
			user.UserName = name;
			user.Description = description;
			await _context.SaveChangesAsync();
			TempData["success"] = "update successfully";
			return RedirectToAction("Index", new { name = user.UserName });

		}
	}
}

