using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QwackX.Api.Models.Dtos;
using QwackX.Api.Services;

namespace QwackX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostViewController : ControllerBase
    {
        private readonly PostViewCache _postViewCache;
        private readonly PostViewSyncService _postViewSyncService;

        public PostViewController(PostViewCache postViewCache, PostViewSyncService postViewSyncService)
        {
            _postViewCache = postViewCache;
            _postViewSyncService = postViewSyncService;
        }

        [HttpPost("increment")]
        public IActionResult IncrementViews(ViewPostDto dto)
        {
            if (dto.PostId <= 0 || dto.UserId <= 0)
            {
                return BadRequest("PostId ou UserId invalide.");
            }

            _postViewCache.AddView(dto.PostId, dto.UserId, dto.ViewedAt);
            return NoContent();
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SyncViews()
        {
            try
            {
                await _postViewSyncService.SyncToDatabase();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erreur lors de la synchronisation des vues.");
            }
        }

    }
}

// var command = new IncrementViewsPosts(dto.PostId, dto.UserId);
//
// var result = _postViewPostViewRepository.Execute(command);
//
// if (result.IsFailure)
//     return  BadRequest($"Erreur lors de l'exécution de la requête: {result.ErrorMessage}, : {result.Exception}");
//
// return NoContent();