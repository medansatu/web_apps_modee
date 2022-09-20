using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using final_project.Data.WishlistRepo;
using final_project.Dtos.Wishlist;
using final_project.Models;
using final_project.Models.WishlistModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace final_project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    // [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistRepository _wishlistRepository;
        public WishlistController( IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
            
        }

        
        [HttpPost("AddToWishlist")]
        public async Task<ActionResult<ServiceResponse<WishlistDTO>>> AddWishlist(int productId)
        {
            var response = await _wishlistRepository.AddWishlist(productId);
            return Ok(response);
        }

        [HttpGet("GetWishlist")]
        public async Task<ActionResult<ServiceResponse<Wishlist>>> GetWishlist()
        {
            var response = await _wishlistRepository.GetWishlist();
            return Ok(response);
        }

        [HttpDelete("DeleteWishlist")]
        public async Task<ActionResult<ServiceResponse<WishlistDTO>>> DeleteWishlist(int id)
        {
            var response = await _wishlistRepository.DeleteWishlist(id);
            
            return Ok(response);
        }
    }
}