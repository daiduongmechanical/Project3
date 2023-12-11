using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Data;
using Project3.Shared;

namespace Project3.Controllers
{
    [Authorize(Policy = "AdminOrManager")]
    public class Total
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int TotalPrice { get; set; }
    }

    public class AdminChartController : BaseController
    {
        public AdminChartController(MyDbContext context) : base(context)
        {
        }

        [Route("/admin/chart")]
        public async Task<IActionResult> Index(string? type, int? year)
        {
            var ListType = await _context.Services.ToListAsync();
            if (type == null && year == null)
            {
                year = DateTime.Now.Year;
                type = $"{ListType.First().Name}_s{ListType.First().Id}";
            }
            var id = type.Split("_s")[1];

            var data = await _context.ServiceRegistereds
                .Include(s => s.ServicePrice)
     .Where(s => s.ServicePrice.ServiceId == int.Parse(id))
     .GroupBy(x => new { x.CreatedDate.Year, x.CreatedDate.Month })
     .Select(group => new Total
     {
         Year = group.Key.Year,
         Month = group.Key.Month,
         TotalPrice = group.Sum(x => x.ServicePrice.Price)
     })
     .ToListAsync();

            ViewData["type"] = type;
            ViewData["year"] = year;
            ViewData["listdata"] = ListType;
            return View(data);
        }
    }
}