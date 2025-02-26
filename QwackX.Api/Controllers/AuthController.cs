using CommandQuerySeparation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QwackX.Api.Domain.Commands;
using QwackX.Api.Domain.Entities;
using QwackX.Api.Domain.Queries;
using QwackX.Api.Domain.Repositories;
using QwackX.Api.Models.Dtos;
using QwackX.Api.Models.Mappers;
using QwackX.Api.Infrastructure;

namespace QwackX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(IAuthRepository repository, ITokenRepository tokenRepository)
        {
            _authRepository = repository;
            _tokenRepository = tokenRepository;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUserDto dto)
        {
            try
            {
                Result<User> result = _authRepository.Execute(new LoginUserQuery(dto.Email, dto.Password));

                if (result.IsFailure && result.ErrorMessage == "Email et mot de passe incorrecte")
                {
                    return Unauthorized(new { Message = result.ErrorMessage });
                }

                if (result.IsFailure)
                {
                    return BadRequest(new { result.ErrorMessage });
                }

                UserDto userDto = result.Content.ToUserDto();
                _tokenRepository.ApplyToken(userDto);

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de l'exécution de la requête: {ex.Message}");
            }
        }
        
        [HttpPost("register")]
        public IActionResult Register(RegisterUserDto dto)
        {
            try
            {
                Result result = _authRepository.Execute(new RegisterUserCommand(dto.Username, dto.Email, dto.Password));

                if (result.IsFailure)
                {
                    return BadRequest(new { result.ErrorMessage });
                }
                
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de l'exécution de la requête: {ex.Message}");
            }
        }
    }
}