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

        public async Task<ServiceResponse<List<ProductDTO>>> FindProduct(string product)
        {
            var response = new ServiceResponse<List<ProductDTO>>();

            List<Product> products = await _context.Products.Where(item => item.ProductName.ToLower().Contains(product.ToLower())).ToListAsync();
            
            if(products != null)
            {
                List<ProductDTO> listProduct = _mapper.Map<List<ProductDTO>>(products);

                response.Data = listProduct;
                response.Message = "Searched Item!";
                return response;
            }
            
            response.Success = false;
            response.Message = "Item Not Found!";
            return response;

        }

        public async Task<ServiceResponse<List<ProductDTO>>> GetAllItem()
        {
            var response = new ServiceResponse<List<ProductDTO>>();
            List<Product>? allItem = await _context.Products
                .Include(item => item.Category)
                .ToListAsync();
            //var abc = _mapper.Map<ProductDTO>(allItem);
            response.Data = allItem.Select(item => _mapper.Map<ProductDTO>(item)).ToList();
            response.Message = "Data Retrieved";

            return response;
        }

        public async Task<ServiceResponse<ProductDTO>> GetItembyId(int id)
        {
            var response = new ServiceResponse<ProductDTO> ();

            var item = await _context.Products
                .Include(item => item.Category)
                .Where(item => item.Id == id)
                .FirstOrDefaultAsync();
            
            ProductDTO productDTO = _mapper.Map<ProductDTO>(item);

            response.Data = productDTO;
            response.Message = "Data Retrieved";

            return response;
        }

        public async Task<ServiceResponse<List<ProductDTO>>> GetNewArrival()
        {
            var response = new ServiceResponse<List<ProductDTO>>();

            List<Product> allItem = await _context.Products
                .Include(item => item.Category)
                .ToListAsync();

            List<ProductDTO> newArrival = new List<ProductDTO>();

            for (int i = allItem.Count(); i> (allItem.Count() - 10); --i)
            {
                ProductDTO? product = allItem.Select(item => _mapper.Map<ProductDTO>(item))
                    .FirstOrDefault(item => item.Id == i);

                newArrival.Add(product);
            }

            response.Data = newArrival;
            return response;
        }
    }
}