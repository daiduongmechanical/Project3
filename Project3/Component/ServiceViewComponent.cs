using Microsoft.AspNetCore.Mvc;

namespace Project3.Component
{
    public class ServiceViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var data = HttpContext.Items["ListService"];
            // You can cast the roomData to the appropriate type as needed
            return View(null, data);
        }
    }
}