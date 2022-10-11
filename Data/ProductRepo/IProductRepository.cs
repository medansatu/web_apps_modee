using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using final_project.Dtos.Product;
using final_project.Models;

namespace final_project.Data.ProductRepo
{
    public interface IProductRepository
    {
        // Task<ServiceResponse<int>> PostProduct(string product_name, string description, int price, int stock, string categories, string image_url);
        Task<ServiceResponse<List<ProductDTO>>> GetAllItem();
        Task<ServiceResponse<ProductDTO>> GetItembyId(int id);
        Task<ServiceResponse<List<ProductDTO>>> FindProduct(String product);
        Task<ServiceResponse<List<ProductDTO>>> GetNewArrival();
    }
}