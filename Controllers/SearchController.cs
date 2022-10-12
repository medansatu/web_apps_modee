using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using final_project.Data.ProductRepo;
using final_project.Dtos.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace final_project.Controllers
{
    // [Route("[controller]")]
    public class SearchController : Controller
    {
        private readonly ILogger<SearchController> _logger;
        private readonly IProductRepository _productRespository;

        public SearchController(ILogger<SearchController> logger, IProductRepository productRespository)
        {
            _logger = logger;
            _productRespository = productRespository;
        }

        [HttpPost]
        public async Task<IActionResult> Index(string keyword)
        {
            HttpContext.Session.SetString("keyword", keyword);
            ViewBag.keyword = HttpContext.Session.GetString("keyword");
            var products = await _productRespository.FindProduct(keyword);
            var model = products.Data;
            return View(model);
        }

        // [HttpPost]
        // public async Task<IActionResult> Seacrh(string keyword)
        // {

        // }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}