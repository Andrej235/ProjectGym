using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Services.Update;
using ProjectGym.Utilities;
using System.Diagnostics;
using static ProjectGym.Controllers.UserController;

namespace ProjectGym.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController(IReadService<User> readService,
                          IEntityMapper<User, UserDTO> mapper,
                          ICreateService<User, Guid> createService,
                          ICreateService<Client, Guid> clientCreateService,
                          IReadService<Client> clientReadService,
                          IUpdateService<Client> clientUpdateService) : ControllerBase, ICreateController<User, RegisterDTO, Guid>
    {
        public IReadService<User> ReadService { get; } = readService;
        public IReadService<Client> ClientReadService { get; } = clientReadService;
        public IEntityMapper<User, UserDTO> Mapper { get; } = mapper;
        public ICreateService<User, Guid> CreateService { get; } = createService;
        public ICreateService<Client, Guid> ClientCreateService { get; } = clientCreateService;
        public IUpdateService<Client> ClientUpdateService { get; } = clientUpdateService;

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

            var newEntityId = await CreateService.Add(user);
            if (newEntityId == default)
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
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Error occurred: {ex.Message} \n{ex.InnerException?.Message}");
                return BadRequest($"Error occurred: {ex.Message} \n{ex.InnerException?.Message}");
            }
        }

        [HttpGet("client/{guid}")]
        public async Task<IActionResult> GetUserFromClient(Guid guid)
        {
            try
            {
                Client client = await ClientReadService.Get(c => c.Id == guid, "user");

                if (client.User is null)
                    return NotFound($"Client with id: {guid} is not logged in.");

                return Ok(Mapper.Map(client.User));
            }
            catch (NullReferenceException)
            {
                return NotFound($"Client with id: {guid} was not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<Guid> VerifyClient(Guid? clientGuid, User user)
        {
            Guid res;
            if (clientGuid is not null)
            {
                try
                {
                    var client = await ClientReadService.Get(c => c.Id == clientGuid, "user");
                    client.User = user;
                    await ClientUpdateService.Update(client);
                    res = (Guid)clientGuid;
                }
                catch (NullReferenceException)
                {
                    Client newClient = new()
                    {
                        Id = new Guid(),
                        User = user,
                        UserGUID = user.Id
                    };

                    await ClientCreateService.Add(newClient);
                    res = newClient.Id;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"---> Error occurred: {ex.Message} \n{ex.InnerException?.Message}");
                    throw;
                }
            }
            else
            {
                Client newClient = new()
                {
                    Id = new Guid(),
                    User = user,
                    UserGUID = user.Id
                };

                await ClientCreateService.Add(newClient);
                res = newClient.Id;
            }
            return res;
        }

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
