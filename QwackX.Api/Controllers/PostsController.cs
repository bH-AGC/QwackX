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
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        
        public PostsController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        
        // GET: api/posts
        [HttpGet]
        public IActionResult Get()
        {
            var result = _postRepository.Execute(new ListeTitlePostsQuery());

            if (result.IsSuccess)
            {
                return Ok(result.Content.ToList());
            }
            else
            {
                return BadRequest($"Erreur lors de l'exécution de la requête: {result.ErrorMessage}");
            }
        }
        
        // GET: api/posts
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _postRepository.Execute(new DetailPostQuery(id));

            if (result.IsSuccess)
            {
                return Ok(result.Content);
            }
            else
            {
                return BadRequest($"Erreur lors de l'exécution de la requête: {result.ErrorMessage}");
            }
        }
        
        // POST: api/posts
        [HttpPost]
        public IActionResult Post(AddPostDto dto)
        {
            var command = new AddPostCommand(dto.UserId, dto.Title, dto.Description);
            var result = _postRepository.Execute(command);
    
            if (result.IsFailure)
                return BadRequest($"Erreur lors de l'exécution de la requête: {result.ErrorMessage}");

            return NoContent();
        }
        
        // DELETE: api/posts/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _postRepository.Execute(new DeletePostCommand(id));

            if (result.IsFailure)
                return  BadRequest($"Erreur lors de l'exécution de la requête: {result.ErrorMessage}");
            ;

            return NoContent();
        }
    }
}