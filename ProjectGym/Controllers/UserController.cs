using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Utilities;
using System;
using static ProjectGym.Controllers.UserController;

namespace ProjectGym.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase, ICreateController<User, RegisterDTO>
    {
        private readonly ExerciseContext context;

        public IReadService<User> ReadService { get; }
        public IEntityMapper<User, UserDTO> Mapper { get; }
        public ICreateService<User> CreateService { get; }

        public UserController(ExerciseContext context, IReadService<User> readService, IEntityMapper<User, UserDTO> mapper, ICreateService<User> createService)
        {
            this.context = context;
            ReadService = readService;
            Mapper = mapper;
            CreateService = createService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RegisterDTO userDTO)
        {
            byte[] salt = HashingService.GenerateSalt();
            User user = new()
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Salt = salt,
                PasswordHash = userDTO.Password.HashPassword(salt)
            };

            var success = await CreateService.Add(user);
            if (!success)
                return BadRequest("User already exists");

            return Ok(new LoggedInDTO()
            {
                ClientGuid = await VerifyClient(userDTO.ClientGuid, user),
                User = Mapper.Map(user)
            });
        }


        [HttpPut("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO userDTO)
        {
            var user = await ReadService.Get(x => x.Email == userDTO.Email, "none");
            if (user is null)
                return NotFound("Incorrect email");

            var hash = userDTO.Password.HashPassword(user.Salt);
            if (!user.PasswordHash.SequenceEqual(hash))
                return BadRequest("Incorrect password");

            return Ok(new LoggedInDTO()
            {
                ClientGuid = await VerifyClient(userDTO.ClientGuid, user),
                User = Mapper.Map(user)
            });
        }

        [HttpGet("client/{guid}")]
        public async Task<IActionResult> GetUserFromClient(Guid guid)
        {
            Client? client = await context.Clients.Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == guid);

            if (client is null)
                return NotFound($"Client with {guid} was not found.");

            if (client.User is null)
                return NotFound($"Client with {guid} is not logged in.");

            return Ok(Mapper.Map(client.User));
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


        [HttpGet("{guid}")]
        public async Task<IActionResult> Get(Guid id, [FromQuery] string? include) => Ok(Mapper.Map(await ReadService.Get(x => x.Id == id, include)));

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
    }
}
