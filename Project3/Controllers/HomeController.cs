﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project3.Data;
using Project3.Models;
using Project3.Shared;
using System.Diagnostics;

namespace Project3.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        public HomeController(MyDbContext context) : base(context)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "AdminOrWriterOrmanager")]
        [Route("/admin/home")]
        public IActionResult Admin()
        {
            return View("Privacy");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("unauthozied")]
        public IActionResult UnAuthorize()
        {
            return View("unauthorize");
        }
    }
}