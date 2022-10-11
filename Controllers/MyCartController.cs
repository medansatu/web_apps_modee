using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using final_project.Data.CartRepo;
using final_project.Dtos;
using final_project.Dtos.Cart;
using final_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace final_project.Controllers
{
    // [Route("[controller]")]
    [Authorize]
    public class MyCartController : Controller
    {
        private readonly ILogger<MyCartController> _logger;
        private readonly ICartRepository _cartRepo;

        public MyCartController(ILogger<MyCartController> logger, ICartRepository cartRepo)
        {
            _logger = logger;
            _cartRepo = cartRepo;
        }

        public async Task<IActionResult> Index()
        {
            ServiceResponse<CartProductDTO> cart = await _cartRepo.GetAllCart();            
            var model = cart.Data;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCart(int cartitemid, int quantity)
        {
            CartItemDTO editCart = new CartItemDTO{
                Id = cartitemid,
                Quantity = quantity
            };
            await _cartRepo.EditCart(editCart);
            return RedirectToAction("Index");
        }       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}