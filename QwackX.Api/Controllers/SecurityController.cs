using Microsoft.AspNetCore.Mvc;
using ToolsSecurity;

namespace QwackX.Api.Controllers
{
    [Route("api/security")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly IRsaService _rsaService;

        public SecurityController(IRsaService rsaService)
        {
            _rsaService = rsaService;
        }

        [HttpGet("publickey")]
        public IActionResult GetPublicKey()
        {
            string publicKey = Convert.ToBase64String(_rsaService.PublicKey);
            return Ok(publicKey);
        }
    }

}