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
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(PhoneNumber) || string.IsNullOrWhiteSpace(Address) || string.IsNullOrWhiteSpace(Password))
            {
                ViewData["RegisterFlag"] = "Please fill all the field";
                return View("Index");
            }
            UserRegisterDTO register = new UserRegisterDTO {
                Name = Name,
                Username = Username,
                Email = Email,
                Password = Password,
                Address = Address,
                PhoneNumber = PhoneNumber
            };
            var id = await _authRepository.Register(register);
            if(id.Success == true)
            {
                return RedirectToAction("Index", "Login");
            }
            else{
                ViewData["RegisterFlag"] = "Username already used";
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