using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Utilities;

namespace ProjectGym.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ExerciseContext context;
        public UserController(ExerciseContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewUser([FromBody] RegisterDTO userDTO)
        {
            if (await context.Users.FirstOrDefaultAsync(u => u.Email == userDTO.Email) is not null)
                return BadRequest("User already exists");

            byte[] salt = HashingService.GenerateSalt();
            User user = new()
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Salt = salt,
                PasswordHash = userDTO.Password.HashPassword(salt)
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return Ok(new LoggedInDTO()
            {
                ClientGuid = await VerifyClient(userDTO.ClientGuid, user),
                User = new()
                {
                    Name = user.Name,
                    Email = user.Email
                }
            });
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> GetUser(Guid guid) => Ok(TranslateToDTO(await context.Users.FirstOrDefaultAsync(x => x.Id == guid)));

        [HttpPut("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO userDTO)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == userDTO.Email);
            if (user is null)
                return NotFound("Incorrect email");

            var hash = userDTO.Password.HashPassword(user.Salt);
            if (!user.PasswordHash.SequenceEqual(hash))
                return BadRequest("Incorrect password");

            return Ok(new LoggedInDTO()
            {
                ClientGuid = await VerifyClient(userDTO.ClientGuid, user),
                User = new()
                {
                    Name = user.Name,
                    Email = user.Email
                }
            });
        }

        [HttpGet("client/{guid}")]
        public async Task<IActionResult> GetUserFromClient(Guid guid)
        {
            Client? client = await context.Clients.Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == guid);

            if(client is null)
                return NotFound($"Client with {guid} was not found.");

            if (client.User is null)
                return NotFound($"Client with {guid} is not logged in.");

            return Ok(TranslateToDTO(client.User));
        }

        [HttpPost("client")]
        public async Task<IActionResult> NewClient()
        {
            Client client = new()
            {
                Id = Guid.NewGuid(),
            };
            await context.Clients.AddAsync(client);
            await context.SaveChangesAsync();
            return Ok(client.Id);
        }

        public async Task<Guid> VerifyClient(Guid? clientGuid, User user)
        {
            Guid res;
            if (clientGuid is not null)
            {
                Client? client = await context.Clients.FirstOrDefaultAsync(c => c.Id == clientGuid);
                if (client is not null)
                {
                    client.User = user;
                    res = (Guid)clientGuid;
                }
                else
                {
                    Client newClient = new()
                    {
                        Id = new Guid(),
                        User = user
                    };
                    await context.Clients.AddAsync(newClient);
                    res = newClient.Id;
                }
            }
            else
            {
                Client newClient = new()
                {
                    Id = new Guid(),
                    User = user
                };
                await context.Clients.AddAsync(newClient);
                res = newClient.Id;
            }
            await context.SaveChangesAsync();
            return res;
        }

        public UserDTO? TranslateToDTO(User? user) => user is null ? null : new()
        {
            Name = user.Name,
            Email = user.Email,
        };

        public class RegisterDTO
        {
            public string Name { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string Password { get; set; } = null!;
            public Guid? ClientGuid { get; set; }
        }

        public class LoginDTO
        {
            public string Email { get; set; } = null!;
            public string Password { get; set; } = null!;
            public Guid? ClientGuid { get; set; }
        }

        public class LoggedInDTO
        {
            public Guid ClientGuid { get; set; }
            public UserDTO? User { get; set; }
        }

        public class UserDTO
        {
            public string Name { get; set; } = null!;
            public string Email { get; set; } = null!;
        }
    }
}
