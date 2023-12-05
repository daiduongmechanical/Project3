using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Data;
using Project3.Dtos;
using Project3.Models;
using Project3.Shared;

namespace Project3.Controllers
{
    [Route("admin/user/{action}")]
    public class AdminUserController : BaseController
    {
        public AdminUserController(MyDbContext context) : base(context)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        // ok
        public async Task<IActionResult> listUser()
        {
            var listUser = await _context.Users.ToListAsync();
            return View(listUser);
        }

        //ok
        public async Task<IActionResult> BlockUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsBlocked = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("listUser");
        }

        //ok
        public async Task<IActionResult> UnblockUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsBlocked = false;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("listUser");
        }

        public async Task<IActionResult> SearchUser(string search)
        {
            var users = await _context.Users.ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(u => u.UserName.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View("listUser", users);
        }

        public async Task<IActionResult> listHobbies()
        {
            var listHobbies = await _context.TypeHobbies.ToListAsync();
            return View(listHobbies);
        }

        //Create hobbies
        public IActionResult createHobbies()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> createHobbies(TypeHobbie type)
        {
            if (ModelState.IsValid)
            {
                _context.TypeHobbies.Add(type);
                await _context.SaveChangesAsync();
                return RedirectToAction("listHobbies");
            }
            return View();
        }

        //edit Hobbies
        public async Task<IActionResult> editHobbies(int id)
        {
            var edithob = await _context.TypeHobbies.SingleOrDefaultAsync(u => u.Id == id);
            return View(edithob);
        }

        //Edit hobbies
        [HttpPost]
        public async Task<IActionResult> editHobbies(int id, TypeHobbie typename)
        {
            var edit = await _context.TypeHobbies.FirstOrDefaultAsync(c => c.Id == id);
            if (ModelState.IsValid)
            {
                edit.Name = typename.Name;

                _context.Entry(edit).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return RedirectToAction("listHobbies");
            }

            return View();
        }

        public async Task<IActionResult> deleteHobbies(int id)
        {
            var hob = await _context.TypeHobbies.FindAsync(id);
            if (hob == null)
            {
                return NotFound();
            }
            var check = await _context.Hobbies.Where(h => h.TypeId == id).ToListAsync();
            if (check != null)
            {
                foreach (var i in check)
                {
                    _context.Remove(i);
                }
            }
            await _context.SaveChangesAsync();
            _context.TypeHobbies.Remove(hob);
            await _context.SaveChangesAsync();
            return RedirectToAction("listHobbies");
        }

        //role list
        public async Task<IActionResult> listRole()
        {
            var list = await _context.Roles.ToListAsync();
            return View(list);
        }

        //User role
        public async Task<IActionResult> roleDetail(int id)
        {
            var list = await _context.UserRoles.Where(c => c.RoleId == id).Include(c => c.Role).ToListAsync();

            if (list == null)
            {
                return NotFound();
            }
            return View(list);
        }

        //Hien role list
        public async Task<IActionResult> ListUserRole()
        {
            BaseMethod.ConvertTempData(TempData, ViewData, "success");
            var users = await _context.Users.Include(u => u.Roles).ThenInclude(ur => ur.Role).ToListAsync();
            return View(users);
        }

        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(Role data)
        {
            _context.Roles.Add(data);
            await _context.SaveChangesAsync();
            TempData["success"] = "Create role successfully";
            return RedirectToAction("ListRole");
        }

        //Hien trang edit Role
        public async Task<IActionResult> EditRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        //method
        [HttpPost]
        public async Task<IActionResult> EditRole(UserEditRoleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.Include(u => u.Roles)
                                               .FirstOrDefaultAsync(u => u.Id == viewModel.UserId);

                if (user == null)
                {
                    return NotFound();
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("ListUserRole");
            }

            viewModel.AllRoles = await _context.Roles.ToListAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> EditUserRole(int id)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == id);
            return View(user);
        }
    }
}