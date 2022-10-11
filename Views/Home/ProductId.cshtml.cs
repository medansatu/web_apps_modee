using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace web_apps_modee.Views.Home
{
    public class ProductId : PageModel
    {
        private readonly ILogger<ProductId> _logger;

        public ProductId(ILogger<ProductId> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}