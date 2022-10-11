using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace final_project.Views.Home
{
    public class Bottom : PageModel
    {
        private readonly ILogger<Bottom> _logger;

        public Bottom(ILogger<Bottom> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}