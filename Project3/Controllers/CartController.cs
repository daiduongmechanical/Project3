using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project3.Data;
using Project3.Models;
using Project3.Shared;

namespace Project3.Controllers
{
    [Authorize]
    public class CartController : BaseController
    {
        public CartController(MyDbContext context) : base(context)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? ServiceId)
        {
            if(ServiceId != null)
            {
                var duration = await _context.ServicesPrice.Where(x=>x.Service_Id == ServiceId).ToListAsync();
                List<SelectListItem> customDurationList = duration
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = $"{s.Duration} Months - ${s.Price}" // Customize the display text as needed
                    })
                    .ToList();
                var selectDuration = new SelectList(customDurationList, "Value", "Text");
                ViewBag.selectDuration = selectDuration;


                var services = await _context.Services.ToListAsync();
                var service = await _context.Services.FirstOrDefaultAsync(x=>x.Id == ServiceId);
                var selectService = new SelectList(services, nameof(Service.Id), nameof(Service.Name), service!.Id);
                ViewBag.selectService = selectService;

            }
            else
            {
                var service = await _context.Services.ToListAsync();
                var selectService = new SelectList(service, nameof(Service.Id), nameof(Service.Name));
                ViewBag.selectService = selectService;
            }

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = HttpContext.User.FindFirst(c => c.Type == "id").Value;
                if(userId != null) {
                    var registered = await _context.ServiceRegistereds
                        .Include(x=>x.ServicePrice)
                        .Where(x => x.User_Id == Int32.Parse(userId))
                        .ToListAsync();
                    ViewBag.registered = registered;
                      if(registered != null && registered.Any())
                    {
                        var subtotal = registered.Sum(x => x.ServicePrice.Price);

                        ViewBag.Subtotal = subtotal;
                    }
                    else
                    {
                        ViewBag.Subtotal = 0;
                    }   
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddService(int servicePriceId)
        {
            if(servicePriceId != null)
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var userId = HttpContext.User.FindFirst(c => c.Type == "id").Value;
                    ServiceRegistered newOrder = new()
                    {
                        Service_Price_Id = servicePriceId,
                        User_Id = Int32.Parse(userId),
                        Status = "Pending"
                    };
                    _context.ServiceRegistereds.Add(newOrder);
                    await _context.SaveChangesAsync();
                // return Ok(userId);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteService(int ServiceRegisteredId)
        {
            if(ServiceRegisteredId != null)
            {
                var serviceRegistered = await _context.ServiceRegistereds.FirstOrDefaultAsync(x => x.Id == ServiceRegisteredId);
                if(serviceRegistered != null)
                {
                _context.ServiceRegistereds.Remove(serviceRegistered);
                    await _context.SaveChangesAsync();
                }
            }
                    return RedirectToAction("Index");
        }
    }
}
