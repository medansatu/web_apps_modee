using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace final_backend.Views.Login
{
    public class welcome : PageModel
    {
        private readonly ILogger<welcome> _logger;

        public welcome(ILogger<welcome> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}