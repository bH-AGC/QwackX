using CommandQuerySeparation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QwackX.Api.Domain.Commands;
using QwackX.Api.Domain.Queries;
using QwackX.Api.Domain.Repositories;
using QwackX.Api.Models.Dtos;

namespace QwackX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RepliesController : ControllerBase
    {
        private readonly IReplyRepository _replyRepository;

        public RepliesController(IReplyRepository replyRepository)
        {
            _replyRepository = replyRepository;
        }

        // GET: api/replies
        [HttpGet("{postId}")]
        public IActionResult Get(int postId)
        {
            var result = _replyRepository.Execute(new ListPostRepliesQuery(postId));

            if (result.IsSuccess)
            {
                return Ok(result.Content.ToList());
            }
            else
            {
                return BadRequest($"Erreur lors de l'exécution de la requête: {result.ErrorMessage}");
            }
        }

        // POST: api/replies
        [HttpPost]
        public IActionResult Post(AddReplyDto dto)
        {
            var command = new AddReplyCommand(dto.PostId, dto.UserId, dto.Content);
            Result result = _replyRepository.Execute(command);

            if (result.IsFailure)
                return BadRequest($"Erreur lors de l'exécution de la requête: {result.ErrorMessage}");

            return NoContent();
        }

        // DELETE: api/replies/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Result result = _replyRepository.Execute(new DeleteReplyCommand(id));

            if (result.IsFailure)
                return BadRequest($"Erreur lors de l'exécution de la requête: {result.ErrorMessage}");

            return NoContent();
        }
    }
}