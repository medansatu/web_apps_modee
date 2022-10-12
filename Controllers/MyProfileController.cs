using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using final_project.Data;
using final_project.Dtos.User;
using final_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace final_project.Controllers
{
    // [Route("[controller]")]
    public class MyProfileController : Controller
    {
        private readonly ILogger<MyProfileController> _logger;
        private readonly IAuthRepository _authRepo;

        public MyProfileController(ILogger<MyProfileController> logger, IAuthRepository authRepo)
        {
            _logger = logger;
            _authRepo = authRepo;
        }

        public async Task<IActionResult> Index()
        {
            ServiceResponse<UserDTO> user = await _authRepo.GetUser();
            var model = user.Data;
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}