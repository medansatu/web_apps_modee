using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using final_project.Data.CartRepo;
using final_project.Data.CategoryRepo;
using final_project.Data.ProductRepo;
using final_project.Data.WishlistRepo;
using final_project.Dtos.Cart;
using final_project.Dtos.Category;
using final_project.Dtos.Product;
using final_project.Dtos.Wishlist;
using final_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace final_project.Controllers
{
    // [Route("[controller]")]c 
    public class HomeController : Controller
    {
         private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _catRepo;
        private readonly ICartRepository _cartRepo;
        private readonly IWishlistRepository _wishlistRepo;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepo, ICategoryRepository catRepo, ICartRepository cartRepo, IWishlistRepository wishlistRepo)
        {
            _logger = logger;
            _productRepo = productRepo;
            _catRepo = catRepo;
            _cartRepo = cartRepo;
            _wishlistRepo = wishlistRepo;
        }

        public async Task<IActionResult> Index()
        {
            ServiceResponse<List<ProductDTO>> products = await _productRepo.GetAllItem();
            ServiceResponse<List<CategoryDTO>> categories = await _catRepo.GetAllCategory();
            var lastProd = products.Data.Last();
            // List<ProductDTO> model = products.Data;
            ProductDTO model = lastProd;
            return View(model);
        }

        public async Task<IActionResult> Top()
        {
            ServiceResponse<List<ProductDTO>> products = await _productRepo.GetAllItem();
            var seletedProduct = products.Data.Where(i => i.Category.Id == 1).ToList();
            return View(seletedProduct);
        }

        public async Task<IActionResult> NewArrival()
        {
            ServiceResponse<List<ProductDTO>> products = await _productRepo.GetNewArrival();
            var seletedProduct = products.Data;
            return View(seletedProduct);
        }
        
        public async Task<IActionResult> Bottom()
        {
            ServiceResponse<List<ProductDTO>> products = await _productRepo.GetAllItem();
            var seletedProduct = products.Data.Where(i => i.Category.Id == 2).ToList();
            return View(seletedProduct);
        }

        public async Task<IActionResult> ProductId(int id)
        {
            ServiceResponse<ProductDTO> product = await _productRepo.GetItembyId(id);

            var seletedProduct = product.Data;
            return View(seletedProduct);
        }
        public async Task<IActionResult> Dress()
        {
            ServiceResponse<List<ProductDTO>> products = await _productRepo.GetAllItem();
            var seletedProduct = products.Data.Where(i => i.Category.Id == 3).ToList();
            return View(seletedProduct);
        }

        public async Task<IActionResult> Shoes()
        {
            ServiceResponse<List<ProductDTO>> products = await _productRepo.GetAllItem();
            var seletedProduct = products.Data.Where(i => i.Category.Id == 4).ToList();
            return View(seletedProduct);
        }

        public async Task<IActionResult> Bag()
        {
            ServiceResponse<List<ProductDTO>> products = await _productRepo.GetAllItem();
            var seletedProduct = products.Data.Where(i => i.Category.Id == 5).ToList();
            return View(seletedProduct);
        }

        public async Task<IActionResult> Accessories()
        {
            ServiceResponse<List<ProductDTO>> products = await _productRepo.GetAllItem();
            var seletedProduct = products.Data.Where(i => i.Category.Id == 6).ToList();
            return View(seletedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            AddToCartDTO addCart = new AddToCartDTO {
                ProductId = productId
            };
            ViewBag.tokencart = HttpContext.Session.GetString("token") ?? null;
            if(ViewBag.tokencart == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
            await _cartRepo.AddCart(addCart);
            return RedirectToAction("Index", "MyCart");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist(int productId)
        {
            WishlistItemDTO addWishlist = new WishlistItemDTO {
                ProductId = productId
            };
            ViewBag.tokenwishlist = HttpContext.Session.GetString("token") ?? null;
            if(ViewBag.tokenwishlist == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
            await _wishlistRepo.AddWishlist(addWishlist);
            return RedirectToAction("Index", "MyWishlist");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

    }
}