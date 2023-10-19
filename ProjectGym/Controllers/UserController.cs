using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using System;
using System.Security.Cryptography;

namespace ProjectGym.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ExerciseContext exerciseContext;

        public UserController(ExerciseContext exerciseContext)
        {
            this.exerciseContext = exerciseContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewUser([FromBody] UserDTO userDTO)
        {
            if (await exerciseContext.Users.FirstOrDefaultAsync(u => u.Email == userDTO.Email) is not null)
                return BadRequest("User already exists");

            byte[] salt = GenerateSalt();
            User user = new()
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Salt = salt,
                PasswordHash = HashPassword(userDTO.Password, salt)
            };

            await exerciseContext.Users.AddAsync(user);
            await exerciseContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> GetUser(Guid guid)
        {
            var user = await exerciseContext.Users.FirstOrDefaultAsync(x => x.Id == guid);
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetUser([FromBody] LoginDTO userDTO)
        {
            var user = await exerciseContext.Users.FirstOrDefaultAsync(x => x.Email == userDTO.Email);
            if (user is null)
                return NotFound("Incorrect email");

            var hash = HashPassword(userDTO.Password, user.Salt);
            if (!user.PasswordHash.SequenceEqual(hash))
                BadRequest("Incorrect password");

            return Ok($"Successfully logged in as {user.Name}" + BitConverter.ToString(hash));
        }

        public static byte[] HashPassword(string password, byte[] salt) => SHA256.HashData(CombineBytes(salt, System.Text.Encoding.UTF8.GetBytes(password)));

        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            RandomNumberGenerator.Create().GetBytes(salt);
            return salt;
        }

        private static byte[] CombineBytes(byte[] first, byte[] second)
        {
            byte[] combined = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, combined, 0, first.Length);
            Buffer.BlockCopy(second, 0, combined, first.Length, second.Length);
            return combined;
        }

        public class UserDTO
        {
            public string Name { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string Password { get; set; } = null!;
        }

        public class LoginDTO
        {
            public string Email { get; set; } = null!;
            public string Password { get; set; } = null!;
        }
    }
}
