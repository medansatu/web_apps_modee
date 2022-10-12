using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using final_project.Data;
using final_project.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace final_project.Controllers
{
    // [Route("[controller]")]
    public class RegisterController : Controller
    {
        private readonly ILogger<RegisterController> _logger;
        private readonly IAuthRepository _authRepository;

        public RegisterController(ILogger<RegisterController> logger, IAuthRepository authRepository)
        {
            _logger = logger;
            _authRepository = authRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string Name, string Username, string Email, string PhoneNumber, string Address, string Password)
        {
            UserRegisterDTO register = new UserRegisterDTO {
                Name = Name,
                Username = Username,
                Email = Email,
                Password = Password,
                Address = Address,
                PhoneNumber = PhoneNumber
            };
            var id = await _authRepository.Register(register);
            if(id != null)
            {
                return RedirectToAction("Index", "Login");
            }
            else{
                ViewBag.msg = "Invalid";
                return View("Index");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}