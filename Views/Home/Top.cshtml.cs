using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace final_backend.Views.Home
{
    public class Top : PageModel
    {
        private readonly ILogger<Top> _logger;

        public Top(ILogger<Top> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}