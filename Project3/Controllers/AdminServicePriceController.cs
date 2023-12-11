using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Data;
using Project3.Models;
using Project3.Shared;

namespace Project3.AdminOrManager
{
    [Route("admin/price/{action}")]
    [Authorize(Policy = "AdminOrManager")]
    public class AdminServicePriceController : BaseController
    {
        public AdminServicePriceController(MyDbContext context) : base(context)
        {
        }

        public async Task<IActionResult> Index()
        {
            var servicePrices = await _context.ServicePrice.ToListAsync();
            return View(servicePrices);
        }

        public async Task<IActionResult> Create()
        {
            var service = await _context.Services.ToListAsync();
            ViewData["service"] = service;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServicePrice servicePrice)
        {
            if (ModelState.IsValid)
            {
                _context.ServicePrice.Add(servicePrice);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Content created successfully";
                return RedirectToAction("Index");
            }

            return View(servicePrice);
        }

        public IActionResult Edit(int id)
        {
            var servicePrice = _context.ServicePrice.Find(id);

            if (servicePrice == null)
            {
                return NotFound();
            }

            return View(servicePrice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ServicePrice servicePrice)
        {
            if (id != servicePrice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                servicePrice.UpdatedDate = DateTime.Now;
                _context.Entry(servicePrice).State = EntityState.Modified;
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Content edited successfully";
                return RedirectToAction("Index");
            }

            return View(servicePrice);
        }

        public IActionResult Delete(int id)
        {
            var servicePrice = _context.ServicePrice.Find(id);

            if (servicePrice == null)
            {
                return NotFound();
            }

            return View(servicePrice);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var servicePrice = _context.ServicePrice.Find(id);

            if (servicePrice == null)
            {
                return NotFound();
            }

            _context.ServicePrice.Remove(servicePrice);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}