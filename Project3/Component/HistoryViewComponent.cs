using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Data;
using Project3.Dtos;
using Project3.Models;

namespace Project3.Component
{
    public class HistoryViewComponent : ViewComponent
    {
        private readonly MyDbContext _context;

        public HistoryViewComponent(MyDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(string name)
        {

          var registered =  _context.ServiceRegistereds
                    .Include(x => x.ServicePrice)
                    .ThenInclude(x => x.Service)
                    .ToList();

            return View(null, registered);
        }
    }
}
