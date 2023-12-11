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

        [Authorize(Policy = "AdminOrWriterOrmanager")]
        public async Task<IActionResult> Index()
        {
            var serviceContents = await _context.ServiceContents

                .ToListAsync();

            return View(serviceContents);
        }

        [HttpGet]
        [Authorize(Policy = "AdminOrWriterOrmanager")]
        public async Task<IActionResult> Create()
        {
            var service = await _context.Services.ToListAsync();
            ViewData["service"] = service;
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrWriterOrmanager")]
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
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Content created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        [Authorize(Policy = "AdminOrWriterOrmanager")]
        public async Task<IActionResult> Edit(int id)
        {
            var serviceContent = _context.ServiceContents.Find(id);

            if (serviceContent == null)
            {
                return NotFound();
            }
            var service = await _context.Services.ToListAsync();
            ViewData["service"] = service;
            return View(serviceContent);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrWriterOrmanager")]
        public async Task<IActionResult> Edit(int id, ServiceContent serviceContent, IFormFile? updateImage)
        {
            if (id != serviceContent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (updateImage != null)
                    {
                        var newName = await BaseMethod.UploadImage(updateImage);
                        BaseMethod.DeleteFile(serviceContent.image);
                        serviceContent.image = newName.Text;
                    }

                    serviceContent.UpdatedDate = DateTime.Now;
                    _context.Entry(serviceContent).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Content edited successfully";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency exception if needed
                    throw;
                }
            }

            // If ModelState is not valid, return to the edit view with the existing model
            var service = await _context.Services.ToListAsync();
            ViewData["service"] = service;
            return View(serviceContent);
        }

        [Authorize(Policy = "AdminOrManager")]
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
        [Authorize(Policy = "AdminOrManager")]
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