using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using final_project.Dtos.Wishlist;
using final_project.Models;
using final_project.Models.WishlistModel;
using Microsoft.EntityFrameworkCore;

namespace final_project.Data.WishlistRepo
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor; 
        private readonly IMapper _mapper;
        public WishlistRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User
            .FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<WishlistItemDTO>> AddWishlist(WishlistItemDTO addWishlistItemDTO)
        {
            var response = new ServiceResponse<WishlistItemDTO>();
            
            Wishlist? wishlist = await _context.Wishlists
                .Where(u => u.Id == GetUserId())
                .FirstOrDefaultAsync();
            
            WishlistItem? wishlistItem = await _context.WishlistItems
                .Where(wishlistItem => wishlistItem.Wishlist == wishlist && wishlistItem.ProductId == addWishlistItemDTO.ProductId)
                .FirstOrDefaultAsync();

            if(wishlistItem == null)
            {
                // Product product = await _context.Products
                //     .Where(product => product.Id == Product_Id)
                //     .FirstOrDefaultAsync();

                // WishlistItem addWishlistItem = new WishlistItem 
                // {
                //     ProductId = ProductId,
                //     Wishlist = wishlist
                // };

                WishlistItem newWishlistItem = _mapper.Map<WishlistItem>(addWishlistItemDTO);

                newWishlistItem.Wishlist = wishlist;
                
                _context.WishlistItems.Add(newWishlistItem);
                
                // var wishlistUser = await _context.Wishlists
                //     .Include(item => item.UserWishlist)
                //     .Include(item => item.WishlistItems)
                //     .Where(wishlist => wishlist.Id == GetUserId())
                //     .FirstOrDefaultAsync();

                // wishlistUser.WishlistItems.Add(wishlistItem);

                await _context.SaveChangesAsync();

                WishlistItem? wishlistItem1 = await _context.WishlistItems
                    .Include(p => p.Product)
                    .Where(w => w.Wishlist == wishlist && w.ProductId == addWishlistItemDTO.ProductId)
                    .FirstOrDefaultAsync();
                
                // WishlistItemDTO? wishlistItemDTO = _mapper.Map<WishlistItemDTO>(wishlistItem1); 
                addWishlistItemDTO.Id = wishlistItem1.Id;
                // wishlistItemDTO.Id = wishlistItem1.Id;

                response.Data = addWishlistItemDTO;
                response.Message = "Data Added!";

                return response;
            }

            response.Message = "Product Already On Wishlist!";

            return response;
        }
            
        public async Task<ServiceResponse<WishlistDTO>> GetWishlist()
        {
            var response = new ServiceResponse<WishlistDTO>();

            var result = await _context.Wishlists
                .Include(item => item.UserWishlist)
                .Where(item => item.Id ==  GetUserId())
                .FirstOrDefaultAsync();

            var WishlistItem = await _context.WishlistItems
                .Include(item => item.Product)
                .Include(item => item.Wishlist)
                .Where(item => item.Wishlist == result)
                .ToListAsync();

            List<WishlistItemDTO> wishlistItemDTO = new List<WishlistItemDTO>();
            WishlistDTO wishlistDTO = _mapper.Map<WishlistDTO>(result);

            foreach (var x in WishlistItem)
            {
                var product = await _context.Products.Where(item => item.Id == x.ProductId).FirstOrDefaultAsync();
                WishlistItemDTO wishlistProduct = _mapper.Map<WishlistItemDTO>(x);
                wishlistProduct.ProductName = product.ProductName;
                wishlistProduct.ImageUrl = product.ImageURL;
                wishlistItemDTO.Add(wishlistProduct);
            }

            wishlistDTO.WishlistItems = wishlistItemDTO;
            response.Data = wishlistDTO;
            response.Message = "Data Wishlist Retrieved";

            return response;
        }

        public async Task<ServiceResponse<WishlistItemDTO>> DeleteWishlist(int id)
        {
            var response = new ServiceResponse<WishlistItemDTO>();



            WishlistItem? item = await _context.WishlistItems
                .Where(wishlist => wishlist.Id == id).FirstOrDefaultAsync();

            WishlistItemDTO? wishlistDTO = _mapper.Map<WishlistItemDTO>(item);

            _context.WishlistItems.Remove(item);
            await _context.SaveChangesAsync();


            
            response.Data = wishlistDTO;
            response.Message = "Wishlist Item Removed!";

            return response;
        }
    }
}