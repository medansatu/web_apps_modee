using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using final_project.Data.CategoryRepo;
using final_project.Data.ProductRepo;
using final_project.Dtos.Category;
using final_project.Dtos.Product;
using final_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace final_project.Controllers
{
    // [Route("[controller]")]
    public class ViewAllProductController : Controller
    {
        private readonly ILogger<ViewAllProductController> _logger;
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _catRepo;

        public ViewAllProductController(ILogger<ViewAllProductController> logger, IProductRepository productRepo, ICategoryRepository catRepo)
        {
            _logger = logger;
            _productRepo = productRepo;
            _catRepo = catRepo;
        }

        public async Task<IActionResult> Index()
        {
            ServiceResponse<List<ProductDTO>> products = await _productRepo.GetAllItem();
            ServiceResponse<List<CategoryDTO>> categories = await _catRepo.GetAllCategory();
            // List<ProductDTO> model = products.Data;
            List<CategoryDTO> model = categories.Data;
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}