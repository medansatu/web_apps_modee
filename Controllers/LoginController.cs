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
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IAuthRepository _authRepo;

        public LoginController(ILogger<LoginController> logger, IAuthRepository authRepo)
        {
            _logger = logger;
            _authRepo = authRepo;
        }

        //[HttpPost]
        public IActionResult Index()
        {  
            ViewBag.token = HttpContext.Session.GetString("token");
           if(ViewBag.token == null)
           {
            return View();
           } 
            else
            {
                return RedirectToAction("Index","MyProfile");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewData["LoginFlag"] = "Please fill all the field";
                return View("Index");
            }            
            UserLoginDTO login = new UserLoginDTO {
                Username = username,
                Password = password
            };
            var user  = await _authRepo.Login(login);
            if(user.Data == null)
            {               
                ViewData["LoginFlag"] = "Invalid Username or Password";
                return View("Index");
            }
            else
            {
                HttpContext.Session.SetString("token", user.Data.Token);
                // return RedirectToAction("welcome");
                return RedirectToAction("Index", "Home");                
            }
        }

        public IActionResult Welcome()
        {             
            ViewBag.token = HttpContext.Session.GetString("token");
            return View("Welcome");
        }

        public IActionResult Logout()
        {              
            HttpContext.Session.Remove("token");
            return RedirectToAction("Index");
        }       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}