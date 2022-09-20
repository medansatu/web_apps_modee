using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using final_project.Models;
using final_project.Dtos.Product;
using AutoMapper;

namespace final_project.Data.ProductRepo
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public ProductRepository(DataContext context, IConfiguration configuration, IMapper mapper)
        {
            _mapper = mapper;
            _configuration = configuration;
            _context = context;
        }

        public Task<ServiceResponse<Product>> DeleteCartItem()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<Product>> EditCart(int quantity)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<List<ProductDTO>>> GetAllItem()
        {
            var response = new ServiceResponse<List<ProductDTO>>();
            List<Product>? allItem = await _context.Products
                .Include(item => item.category)
                .ToListAsync();
            //var abc = _mapper.Map<ProductDTO>(allItem);
            response.Data = allItem.Select(item => _mapper.Map<ProductDTO>(item)).ToList();
            response.Message = "Data Retrieved";

            return response;
        }

        public async Task<ServiceResponse<Product>> GetItembyId(int id)
        {
            var response = new ServiceResponse<Product> ();

            var item = await _context.Products
                .Include(item => item.category)
                .Where(item => item.id == id)
                .FirstOrDefaultAsync();

            response.Data = item;
            response.Message = "Data Retrieved";

            return response;
        }
    }
}