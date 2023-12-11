using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Policy = "AdminOrManager")]
        public async Task<IActionResult> ListUser()
        {
            var listUser = await _context.Users
                .Include(u => u.Roles).ThenInclude(role => role.Role)
               .Where(u => !u.Roles.Select(m => m.Role.RoleName).Any(roleName => roleName == "admin"
               || roleName == "manager"))
                .ToListAsync();
            return View(listUser);
        }

        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> ListAdmin()
        {
            var listAdmin = await _context.Users
                .Include(u => u.Roles).ThenInclude(role => role.Role)
                .Where(u => u.Roles.Select(r => r.Role.RoleName).Contains("admin"))
                .ToListAsync();
            return View(listAdmin);
        }

        [Authorize(Policy = "AdminOrManager")]
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

        [Authorize(Policy = "AdminOrManager")]
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

        [Authorize(Policy = "AdminOrManager")]
        public async Task<IActionResult> listHobbies()
        {
            var listHobbies = await _context.TypeHobbies.ToListAsync();
            return View(listHobbies);
        }

        [Authorize(Policy = "AdminOrManager")]
        public IActionResult createHobbies()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrManager")]
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
        [Authorize(Policy = "AdminOrManager")]
        public async Task<IActionResult> editHobbies(int id)
        {
            var edithob = await _context.TypeHobbies.SingleOrDefaultAsync(u => u.Id == id);
            return View(edithob);
        }

        //Edit hobbies
        [HttpPost]
        [Authorize(Policy = "AdminOrManager")]
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

        [Authorize(Policy = "AdminOrManager")]
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
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> listRole()
        {
            var list = await _context.Roles.ToListAsync();
            return View(list);
        }

        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> roleDetail(int id)
        {
            var list = await _context.UserRoles.Where(c => c.RoleId == id).Include(c => c.Role).ToListAsync();

            if (list == null)
            {
                return NotFound();
            }
            return View(list);
        }

        [Authorize(Policy = "AdminOrManager")]
        public async Task<IActionResult> ListUserRole()
        {
            BaseMethod.ConvertTempData(TempData, ViewData, "success");
            var users = await _context.Users.Include(u => u.Roles).ThenInclude(ur => ur.Role).ToListAsync();
            return View(users);
        }

        [Authorize(Policy = "ManagerOnly")]
        public IActionResult CreateRole()
        {
            return View();
        }

        [Authorize(Policy = "ManagerOnly")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(Role data)
        {
            _context.Roles.Add(data);
            await _context.SaveChangesAsync();
            TempData["success"] = "Create role successfully";
            return RedirectToAction("ListRole");
        }

        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> EditRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [Authorize(Policy = "ManagerOnly")]
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

        [Authorize(Policy = "AdminOrManager")]
        public async Task<IActionResult> EditUserRole(int id)
        {
            var listRole = await _context.Roles.Where(r => r.RoleName == "user" || r.RoleName == "writer")
                .ToListAsync();
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == id);
            ViewData["role"] = listRole;
            return View(user);
        }

        [Authorize(Policy = "AdminOrManager")]
        [HttpPost]
        public async Task<IActionResult> EditUserRole(int id, List<int> role)
        {
            var check = await _context.UserRoles.Where(r => r.UserId == id).ToListAsync();
            if (check != null)
            {
                foreach (var i in check)
                {
                    _context.Remove(i);
                }
            }
            foreach (var i in role)
            {
                _context.Add(new UserRole { UserId = id, RoleId = i });
            }
            await _context.SaveChangesAsync();
            TempData["success"] = "update role successfully";
            return RedirectToAction("listUser");
        }

        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> EditAdminRole(int id)
        {
            var listRole = await _context.Roles
                .ToListAsync();
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == id);
            ViewData["role"] = listRole;
            return View(user);
        }

        [HttpPost]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> EditAdminRole(int id, List<int> role)
        {
            var check = await _context.UserRoles.Where(r => r.UserId == id).ToListAsync();
            if (check != null)
            {
                foreach (var i in check)
                {
                    _context.Remove(i);
                }
            }
            foreach (var i in role)
            {
                _context.Add(new UserRole { UserId = id, RoleId = i });
            }
            await _context.SaveChangesAsync();
            TempData["success"] = "update role successfully";
            return RedirectToAction("ListAdmin");
        }
    }
}