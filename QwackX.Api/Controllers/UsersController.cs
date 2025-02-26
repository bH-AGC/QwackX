using CommandQuerySeparation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QwackX.Api.Domain.Commands;
using QwackX.Api.Domain.Entities;
using QwackX.Api.Domain.Queries;
using QwackX.Api.Domain.Repositories;
using QwackX.Api.Models.Dtos;

namespace QwackX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        // GET: api/user
        [HttpGet]
        public IActionResult Get()
        {
            Result<IEnumerable<User?>> result = _userRepository.Execute(new ListUsersQuery());

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
            Result<User?> result = _userRepository.Execute(new DetailUserQuery(id));

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
            AddUserCommand command = new AddUserCommand(dto.Username, dto.Email, dto.Password);
            Result result = _userRepository.Execute(command);
    
            if (result.IsFailure)
                return BadRequest($"Erreur lors de l'exécution de la requête: {result.ErrorMessage}");

            return NoContent();
        }
        
        // PUT/PATCH : api/user
        [HttpPut]
        [HttpPatch]
        public IActionResult Put(EditUserDto dto)
        {
            Result result = _userRepository.Execute(new EditUserCommand(dto.UserId, dto.Username, dto.Email, dto.Password));
            if(result.IsFailure)
                return BadRequest(dto);

            return NoContent();
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Result result = _userRepository.Execute(new DeleteUserCommand(id));

            if (result.IsFailure)
                return  BadRequest($"Erreur lors de l'exécution de la requête: {result.ErrorMessage}");
            ;

            return NoContent();
        }
    }
}
