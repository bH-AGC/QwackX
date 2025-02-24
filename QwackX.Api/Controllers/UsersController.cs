using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QwackX.Api.Domain.Commands;
using QwackX.Api.Domain.Queries;
using QwackX.Api.Domain.Repositories;
using QwackX.Api.Infrastructure;
using QwackX.Api.Models.Dtos;

namespace QwackX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        
        public UserController(IUserRepository userRepository, ITokenRepository tokenRepository)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            
            Console.WriteLine("✅ UserController instancié !");
        }
        
        // GET: api/user
        [HttpGet]
        public IActionResult Get()
        {
            var user = _tokenRepository.User;
            
            if (user == null)
            {
                Console.WriteLine("⚠️ Aucun utilisateur trouvé !");
                return Unauthorized("Token invalide ou manquant.");
            }
            
            var result = _userRepository.Execute(new ListUsersQuery());

            if (result.IsSuccess)
            {
                return Ok(result.Content.ToList());
            }
            else
            {
                return BadRequest($"Erreur lors de l'exécution de la requête: {result.ErrorMessage}");
            }
        }

        // GET: api/user
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _userRepository.Execute(new DetailUserQuery(id));

            if (result.IsSuccess)
            {
                return Ok(result.Content);
            }
            else
            {
                return BadRequest($"Erreur lors de l'exécution de la requête: {result.ErrorMessage}");
            }
        }

        // POST: api/user
        [HttpPost]
        public IActionResult Post(AddUserDto dto)
        {
            // // Hachage du mot de passe
            // string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var command = new AddUserCommand(dto.Username, dto.Email, dto.Password);
            var result = _userRepository.Execute(command);
    
            if (result.IsFailure)
                return BadRequest($"Erreur lors de l'exécution de la requête: {result.ErrorMessage}");

            return NoContent();
        }
        
        // PUT/PATCH : api/user
        [HttpPut]
        [HttpPatch]
        public IActionResult Put(EditUserDto dto)
        {
            // string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            //
            var result = _userRepository.Execute(new EditUserCommand(dto.Id, dto.Username, dto.Email, dto.Password));
            if(result.IsFailure)
                return BadRequest(dto);

            return NoContent();
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _userRepository.Execute(new DeleteUserCommand(id));

            if (result.IsFailure)
                return  BadRequest($"Erreur lors de l'exécution de la requête: {result.ErrorMessage}");
            ;

            return NoContent();
        }
    }
}
