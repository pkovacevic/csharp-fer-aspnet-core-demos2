using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager )
        {
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> TopSecret()
        {
            // you can check if client is authenticated this way.
            // here it will be always true since [Authorize] tag won't let
            // anonymous users in.
            bool alwaysTrueHere = HttpContext.User.Identity.IsAuthenticated;

            // fetch current user information this way.
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return View(user);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
