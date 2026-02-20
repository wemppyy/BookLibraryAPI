using Azure.Core;
using BookLibraryAPI.Abstracts;
using BookLibraryAPI.CoreConfig;
using BookLibraryAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookLibraryAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserSerivce _userService;
        private readonly JwtToken _jwt;

        public AuthController(IUserSerivce userService, IConfiguration config)
        {
            _userService = userService;
            _jwt = new JwtToken(config);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register(RegisterRequest request)
        {
            Console.WriteLine($"[DEBUG] AuthController.Register called with Email: {request?.Email}");
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email and Password are required.");
            }

            var ok = _userService.Create(request);
            
            return ok 
                ? Ok() 
                : Conflict("User already exists.");
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login(LoginRequest request)
        {
            Console.WriteLine($"[DEBUG] AuthController.Login called with Email: {request?.Email}");
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email and Password are required.");
            }

            var user = _userService.Find(request.Email);

            if (user == null || !_userService.CheckPassword(user, request.Password))
            {
                return Unauthorized();
            }

            return Ok(new AuthResponse
            {
                AccessToken = _jwt.Create(user),
                Role = user.Role,
                Email = user.Email
            });
        }

        [HttpPost("createAdmin")]
        [Authorize(Roles=Roles.Admin)]
        public IActionResult CreateAdmin(RegisterRequest request)
        {
            Console.WriteLine($"[DEBUG] AuthController.CreateAdmin called with Email: {request?.Email}");
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email and Password are required.");
            }

            var ok = _userService.Create(request, Roles.Admin);

            return ok 
                ? Ok() 
                : Conflict("User already exists.");
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult GetUsers()
        {
            Console.WriteLine("[DEBUG] AuthController.GetUsers called");
            return Ok(_userService.GetAll());
        }
    }
}
