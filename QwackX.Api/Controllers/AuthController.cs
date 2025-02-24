using CommandQuerySeparation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Login(LoginDto dto)
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
                    return BadRequest(result);
                }

                UserDto userDto = result.Content.ToUserDto();
                _tokenRepository.ApplyToken(userDto);
                
                // Console.WriteLine(_tokenRepository.User.ToString());

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de l'exécution de la requête: {ex.Message}");
            }
        }
    }
}