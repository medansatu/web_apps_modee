using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace final_project.Views.Home
{
    public class Accessories : PageModel
    {
        private readonly ILogger<Accessories> _logger;

        public Accessories(ILogger<Accessories> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}