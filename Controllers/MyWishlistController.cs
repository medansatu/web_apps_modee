using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using final_project.Data.WishlistRepo;
using final_project.Dtos.Wishlist;
using final_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace final_backend.Controllers
{
    [Authorize]
    // [Route("[controller]")]
    public class MyWishlistController : Controller
    {
        private readonly ILogger<MyWishlistController> _logger;
        private readonly IWishlistRepository _wishlistRepo;

        public MyWishlistController(ILogger<MyWishlistController> logger, IWishlistRepository wishlistRepo)
        {
            _logger = logger;
            _wishlistRepo = wishlistRepo;
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            ViewBag.tokenwishlists = HttpContext.Session.GetString("token") ?? null;
            if(ViewBag.tokenwishlists == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
            ServiceResponse<WishlistDTO> wishlist = await _wishlistRepo.GetWishlist();
            var model = wishlist.Data;
            return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteWishlist(int id)
        {
            await _wishlistRepo.DeleteWishlist(id);
            return RedirectToAction("index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}