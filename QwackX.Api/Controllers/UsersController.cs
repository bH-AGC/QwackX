using CommandQuerySeparation.Commands;
using CommandQuerySeparation.Queries;
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
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;
        
        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }
        
        // GET: api/user
        [HttpGet]
        public IActionResult Get()
        {
            var result = _repository.Execute(new ListUsersQuery());

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
            var result = _repository.Execute(new DetailUserQuery(id));

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
            var command = new AddUserCommand(dto.Username, dto.Email, dto.PasswordHash);
            var result = _repository.Execute(command);
            
            if (result.IsFailure)
                return BadRequest($"Erreur lors de l'exécution de la requête: {result.ErrorMessage}");

            return NoContent();
        }
        
        // PUT/PATCH : api/user
        [HttpPut]
        [HttpPatch]
        public IActionResult Put(EditUserDto dto)
        {
            var result = _repository.Execute(new EditUserCommand(dto.Id, dto.Username, dto.Email, dto.PasswordHash));
            if(result.IsFailure)
                return BadRequest(dto);

            return NoContent();
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _repository.Execute(new DeleteUserCommand(id));

            if (result.IsFailure)
                return BadRequest(result.ErrorMessage);

            return BadRequest($"Erreur lors de l'exécution de la requête: {result.ErrorMessage}");
        }
    }
}
