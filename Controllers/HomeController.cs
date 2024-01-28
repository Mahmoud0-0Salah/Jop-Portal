﻿using Jop_Portal.Data;
using Jop_Portal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Jop_Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, UserManager<IdentityUser> UserManager)
        {
            _logger = logger;
            _context = context;
            _userManager = UserManager;
        }

        public IActionResult Index(string? search)
        {
            string id = _userManager.GetUserId(User);
            var model = _context.Offers.Where(o => (o.Active&& o.Available));
            if (search != null)
            {
                    model = SearchOffers(model, search);
            }
            if (id != null)
            {
                ViewData["Name"] = _context.Account.Single(a => a.Id == id).Name;
                ViewData["Photo"] = _context.Account.Single(a => a.Id == id).Photo;
                ViewData["About"] = _context.Account.Single(a => a.Id == id).About;
            }
           return View(model);
        }

        public IQueryable<Offers> SearchOffers(IQueryable<Offers> offers, string searchInput)
        {
            // using FreeText function require a full-text index to be configured.

                var result = offers.Where(o => o.Description.Contains(searchInput)
                                                            || o.JobTittle.Contains(searchInput));
                return result;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}