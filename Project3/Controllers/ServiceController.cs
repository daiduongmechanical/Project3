using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Data;
using Project3.Models;
using Project3.Shared;

namespace Project3.Controllers
{
    public class ServiceController : BaseController
    {
        public ServiceController(MyDbContext context) : base(context)
        {
        }

        [Route("/Service/{Type}/{Id?}")]
        public async Task<IActionResult> Index(string Type, int? Id)
        {
            var serviceId = Type.Split("_s")[1];
            var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

            var check = await _context.ServiceRegistereds.Include(s => s.ServicePrice)
                .FirstOrDefaultAsync(s => s.UserId == int.Parse(userId)
                && s.ServicePrice.ServiceId == int.Parse(serviceId)
                && s.Status == "Purchased"
                );

            if (check != null)
            {
                var checkExpiration = BaseMethod.ConvertToUnixTimestamp(DateTime.Now) - BaseMethod.ConvertToUnixTimestamp(check.CreatedDate);
                if (checkExpiration < check.ServicePrice.Duration * 60 * 60 * 24 * 30)
                {
                    if (Id == null)
                    {
                        var contents = await _context.ServiceContents.Where(s => s.ServiceId == int.Parse(serviceId)).ToListAsync();
                        ViewData["serviceid"] = Type;
                        return View(contents);
                    }
                    else
                    {
                        var unit = _context.ServiceContents.FirstOrDefaultAsync(s => s.Id == Id);

                        return View(unit);
                    }
                }
                else
                {
                    TempData["error"] = "Please register service!";
                    return RedirectToAction("Index", "Cart");
                }
            }
            else
            {
                TempData["error"] = "Please register service!";
                return RedirectToAction("Index", "Cart");
            }
        }
    }
}