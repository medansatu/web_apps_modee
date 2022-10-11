using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using final_project.Data.CartRepo;
using final_project.Data.ProductRepo;
using final_project.Data.WishlistRepo;
using final_project.Dtos;
using final_project.Dtos.Cart;
using final_project.Dtos.Product;
using final_project.Models;
using final_project.Models.CartModel;
using final_project.Models.WishlistModel;
using Microsoft.AspNetCore.Mvc;

namespace final_project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepo;
        public ProductController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
            
        }

        [HttpGet("GetAllProduct")]
        public async Task<ActionResult<ServiceResponse<List<ProductDTO>>>> GetAllItem()
        {
            var response = await _productRepo.GetAllItem();
            
            return Ok(response);
        }

        [HttpGet("GetProductItemId/{id}")]
        public async Task<ActionResult<ServiceResponse<ProductDTO>>> GetItembyId(int id)
        {
            var response = await _productRepo.GetItembyId(id);
            
            return Ok(response);
        }

        [HttpGet("SearchProduct")]
        public async Task<ActionResult<ServiceResponse<ProductDTO>>> FindProduct(String product)
        {
            var response = await _productRepo.FindProduct(product);

            if(response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("NewArrival")]
        public async Task<ActionResult<ServiceResponse<List<ProductDTO>>>> GetNewArrival()
        {
            var response = await _productRepo.GetNewArrival();

            return Ok(response);
        }
        
    }
}