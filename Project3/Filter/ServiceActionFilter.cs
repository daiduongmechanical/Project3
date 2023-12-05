using Microsoft.AspNetCore.Mvc.Filters;
using Project3.Data;

namespace Project3.Filter
{
    public class ServiceActionFilter : IActionFilter
    {
        private readonly MyDbContext _context;

        public ServiceActionFilter(MyDbContext context)
        {
            _context = context;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var data = _context.Services.ToList();
            context.HttpContext.Items["ListService"] = data;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}