using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace final_project.Dtos.Wishlist
{
    public class WishlistItemDTO
    {
        public int Id { get; set; }
        
        public int ProductId { get; set; }
        public string ProductName {get; set; } = string.Empty;
        public string ImageUrl {get; set; } = string.Empty;
    }
}