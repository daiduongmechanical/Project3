using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Data;
using Project3.Models;
using Project3.Shared;

namespace Project3.Controllers
{
    [Route("admin/service/{action}")]
    [Authorize(Policy = "AdminOrManager")]
    public class AdminServiceController : BaseController
    {
        public AdminServiceController(MyDbContext context) : base(context)
        {
        }

        public async Task<IActionResult> Index()
        {
            var services = await _context.Services.ToListAsync();
            return View(services);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Services.Add(service);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Content created successfully";
                return RedirectToAction("Index");
            }

            return View(service);
        }

        public IActionResult Edit(int id)
        {
            var service = _context.Services.Find(id);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Service service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                service.UpdatedDate = DateTime.Now; // Update the UpdatedDate property
                _context.Entry(service).State = EntityState.Modified;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Content edited successfully";
                return RedirectToAction("Index");
            }

            return View(service);
        }

        public IActionResult Delete(int id)
        {
            var service = _context.Services.Find(id);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var service = _context.Services.Find(id);

            if (service == null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}