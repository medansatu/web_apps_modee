using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using final_project.Data;
using final_project.Dtos.User;
using final_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace final_project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthController(IAuthRepository authRepo, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _authRepo = authRepo;
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDTO register)
        {
            var response = await _authRepo.Register(register);
            if(!response.Success)
            {
                response.Message = "User Already Used!";
                return BadRequest(response);
            }
            return Ok(response);
            //return Ok(await _authRepo.Register(register));
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<UserDTO>>> Login(UserLoginDTO login)
        {
            var response = await _authRepo.Login(login);
            if(!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetProfile")]
        public async Task<ActionResult<ServiceResponse<UserDTO>>> GetUser()
        {
            return Ok(await _authRepo.GetUser());
        }
    }
}