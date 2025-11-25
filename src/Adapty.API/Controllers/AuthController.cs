using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Adapty.API.Data;
using Adapty.API.Models;
using Adapty.API.DTOs;
using Adapty.API.Services; // Importante

namespace Adapty.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthService _authService; // Injeta o Service

        public AuthController(AppDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequestDto request)
        {
            if (_context.Users.Any(u => u.Email == request.Email))
            {
                return BadRequest("Usuário já cadastrado.");
            }

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Role = request.Role,
                PasswordHash = request.Password 
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            if (user == null || user.PasswordHash != request.Password)
            {
                return Unauthorized("E-mail ou senha inválidos.");
            }
            Console.WriteLine($"Senha enviada: '{request.Password}'");
            Console.WriteLine($"Senha salva:   '{user.PasswordHash}'");

            return Ok(new { message = "Usuário registrado com sucesso!" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

            if (user == null || user.PasswordHash != request.Password)
            {
                return Unauthorized("E-mail ou senha inválidos.");
            }

            // O Controller pede o token para o Service. Ele não sabe COMO é feito.
            var token = _authService.GenerateJwtToken(user);

            return Ok(new { token = token, user = new { user.Name, user.Email } });
        }
    }
}