using CommandQuerySeparation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QwackX.Api.Domain.Commands;
using QwackX.Api.Domain.Repositories;
using QwackX.Api.Models.Dtos;

namespace QwackX.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LikesController : Controller
{
    private readonly ILikeRepository _likeRepository;
        
    public LikesController(ILikeRepository likeRepository)
    {
        _likeRepository = likeRepository;
    }
    
    // POST: api/likePost
    [HttpPost("like")]
    public IActionResult Post(LikeDto dto)
    {
        LikeCommand command = new LikeCommand(dto.UserId, dto.EntityId, dto.EntityType);
        Result result = _likeRepository.Execute(command);
    
        if (result.IsFailure)
            return BadRequest($"Erreur lors de l'exécution de la requête: {result.ErrorMessage}, : {result.Exception}");

        return NoContent();
    }
}