using Microsoft.AspNetCore.Mvc;
using ToolSecurity;

namespace QwackX.Api.Controllers
{
    [Route("api/security")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly IRsaRepository _rsaRepository;

        public SecurityController(IRsaRepository rsaRepository)
        {
            _rsaRepository = rsaRepository;
        }

        [HttpGet("publickey")]
        public IActionResult GetPublicKey()
        {
            string publicKey = Convert.ToBase64String(_rsaRepository.PublicKey);
            return Ok(publicKey);
        }
    }

}