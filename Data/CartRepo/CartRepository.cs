using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using final_project.Dtos;
using final_project.Dtos.Cart;
using final_project.Dtos.Product;
using final_project.Models;
using final_project.Models.CartModel;
using Microsoft.EntityFrameworkCore;

namespace final_project.Data.CartRepo
{
    
    public class CartRepository : ICartRepository
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public CartRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
           _context = context;
            
        }
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User
            .FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<CartProductDTO>> GetAllCart()
        {
            var response = new ServiceResponse<CartProductDTO>();

            var cart = await _context.Carts
                .Include(item => item.UserCart)
                .Where(u => u.Id == GetUserId())
                .FirstOrDefaultAsync();
                
            var cartItem = await _context.CartItems
                .Include(item => item.Product)
                .Include(item => item.Cart)
                .Where(item => item.Cart == cart)
                .ToListAsync();
            
            List<ProductCartDTO> productCartDTO = new List<ProductCartDTO>();

            var cartx = cartItem.ToArray();

            foreach(var x in cartItem)
            {
                var product = await _context.Products.Where(item => item.Id == x.ProductId).FirstOrDefaultAsync();
                ProductCartDTO productCart = new ProductCartDTO
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Price = product.Price,
                    ProductName = product.ProductName,
                    ImageUrl = product.ImageURL,
                };
                productCartDTO.Add(productCart);
            }

            CartProductDTO cartDTO = new CartProductDTO
            {
                Id = GetUserId(),
                CartItemList = productCartDTO
            };

            response.Data = cartDTO;
            response.Message = "Data Retrieved";

            return response;
        }

        public async Task<ServiceResponse<AddToCartDTO>> AddCart(AddToCartDTO addToCartDTO)
        {
            var response = new ServiceResponse<AddToCartDTO>();

            Cart? cartUser = await _context.Carts
                .Where(u => u.Id == GetUserId())
                .FirstOrDefaultAsync();

            CartItem? cartItemUser = await _context.CartItems
                .Where(cartitem => cartitem.Cart == cartUser && cartitem.ProductId == addToCartDTO.ProductId)
                .FirstOrDefaultAsync();

            if(cartItemUser == null)
            {
                CartItem cartItem = _mapper.Map<CartItem>(addToCartDTO);
                // cartUser.CartItems.Add
                cartItem.Cart = cartUser;
                // if()
                _context.CartItems.Add(cartItem);

                await _context.SaveChangesAsync();

                CartItem? item = await _context.CartItems
                    .Where(cartitem => cartitem.Cart == cartUser && cartitem.ProductId == addToCartDTO.ProductId)
                    .FirstOrDefaultAsync();

                addToCartDTO.Id = item.Id;

                response.Data = addToCartDTO;
                response.Message = "Item Added To Cart!";
                //response.Data = item.Select(item => _mapper.Map<AddToCartDTO>(item)).Tolist();

                return response;
            }

            // int qty = cartItemUser.Quantity + 1;
            // cartItemUser.Quantity = qty;
            cartItemUser.Quantity++;

            await _context.SaveChangesAsync();

            AddToCartDTO addToCart = _mapper.Map<AddToCartDTO>(cartItemUser);
            
            response.Data = addToCart;
            response.Message = "Item Quantity Added!";

            return response;
        }

        public async Task<ServiceResponse<CartItem>> EditCart(CartItemDTO cartItemDTO)
        {
            var response = new ServiceResponse<CartItem> ();
            
            CartItem? item = await _context.CartItems
                .Where(cart => cart.Id == cartItemDTO.Id).FirstOrDefaultAsync();
            if (cartItemDTO.Quantity == 0)
                {
                    _context.Remove(item);
                }
            item.Quantity = cartItemDTO.Quantity;
            item.Notes = cartItemDTO.Notes;
            await _context.SaveChangesAsync();
            response.Data = item;
            response.Message = "Item Quantity Added!";
            return response;
        }

        public async Task<ServiceResponse<CartItemDTO>> DeleteCart(int id)
        {
            var response = new ServiceResponse<CartItemDTO>();

            CartItem? item = await _context.CartItems
                .Where(cart => cart.Id == id).FirstOrDefaultAsync();
            
            if(item != null)
            {
                CartItemDTO? cartItem = _mapper.Map<CartItemDTO>(item);
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
                response.Data = cartItem;
                response.Message = "Item Deleted!";
                return response;
            }

            response.Success = false;
            response.Message = "ID Not found";
            return response;
        }

        public async Task<ServiceResponse<int>> GrandTotal()
        {
            var response = new ServiceResponse<int>();

            Cart? cart = await _context.Carts
                .Include(p => p.CartItems)
                .Where(u => u.Id == GetUserId())
                .FirstOrDefaultAsync();

            var  totalList = new List<int>();

            if(cart.CartItems.Count() != 0)
            {
                // List<CartItem> cartItem = await _context.CartItems
                //     .Where(i => i.Cart == cart)
                //     .ToListAsync();

                foreach(var i in cart.CartItems)
                {
                    Product product = await _context.Products.Where(x => x.Id == i.ProductId).FirstOrDefaultAsync();
                    int total = i.Quantity * product.Price;
                    totalList.Add(total);
                }
                
                int grandTotal = totalList.Sum();

                response.Data = grandTotal;
                response.Message = "Total Cart Retrieved!";
                return response;
            }
            
            response.Data = 0;
            response.Message = "Cart Is empty!";
            return response;

        }
    }
}