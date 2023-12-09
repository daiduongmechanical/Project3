using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Data;
using Project3.Dtos;
using Project3.Models;
using Project3.Shared;

namespace Project3.Controllers
{
    [Route("admin/content/{action}")]
    public class AdminContentController : BaseController
    {
        public AdminContentController(MyDbContext context) : base(context)
        {
        }

        public async Task<IActionResult> Index()
        {
            var serviceContents = await _context.ServiceContents

                .ToListAsync();

            return View(serviceContents);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var service = await _context.Services.ToListAsync();
            ViewData["service"] = service;
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(ContentDto data)
        {

            if (ModelState.IsValid)
            {
                var imageName = await BaseMethod.UploadImage(data.Images);
                if (imageName.Result != true)
                {
                    TempData["error"] = "Error has occured uploading image. Please try again! ";
                    return RedirectToAction("Index");
                }

                var content = new ServiceContent()
                {
                    ServiceId = data.ServiceId,
                    image = imageName.Text,

                    Content = data.Content,
                    Title = data.Title,
                   

                };
                _context.Add(content);
              await  _context.SaveChangesAsync();
                TempData["success"] = "processed successfully ";
                return RedirectToAction("Index");



               
            }
            return View();
        }
        public IActionResult Edit(int id)
        {
            var serviceContent = _context.ServiceContents.Find(id);

            if (serviceContent == null)
            {
                return NotFound();
            }

            return View(serviceContent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ServiceContent serviceContent)
        {
            if (id != serviceContent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                serviceContent.UpdatedDate = DateTime.Now;
                _context.Entry(serviceContent).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(serviceContent);
        }

        public IActionResult Delete(int id)
        {
            var serviceContent = _context.ServiceContents.Find(id);

            if (serviceContent == null)
            {
                return NotFound();
            }

            return View(serviceContent);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var serviceContent = _context.ServiceContents.Find(id);

            if (serviceContent == null)
            {
                return NotFound();
            }

            _context.ServiceContents.Remove(serviceContent);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}