using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using QwackX.Api.Controllers;

namespace QwackX.Api.Infrastructure
{

    public class TokenService : ITokenRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;

        public TokenService(IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            _configuration = configuration;
            _httpContext = httpContext;

            Console.WriteLine("✅ TokenService instancié !");
        }

        public UserDto? User
        {
            get
            {
                string? token = ExtractToken();

                if (token is null)
                {
                    return null;
                }

                return ExtractDataFromToken(token);
            }
        }

        public void ApplyToken(UserDto user)
        {
            try
            {

                SymmetricSecurityKey key =
                    new SymmetricSecurityKey(Encoding.Default.GetBytes(_configuration["JwtSettings:SecretKey"]));
                SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Issuer"],
                    audience: _configuration["Audience"],
                    claims:
                    [
                        new Claim("Id", user.Id.ToString()),
                        new Claim("Username", user.Username),
                        new Claim("Email", user.Email),
                        new Claim("CreatedAt", user.CreateAt.ToString("yyyy-MM-dd HH:mm:ss")),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    ],
                    signingCredentials: creds);

                user.Token = new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                // Log l'exception pour voir ce qui échoue
                throw new InvalidOperationException("Erreur lors de la création du token JWT", ex);
            }
        }

        private string? ExtractToken()
        {
            const string prefix = "Bearer ";
            HttpContext? httpContext = _httpContext.HttpContext;
            if (httpContext is null)
            {
                throw new InvalidOperationException();
            }

            Console.WriteLine();
            StringValues autorisations = httpContext.Request.Headers["Authorization"];

            string? token = autorisations.SingleOrDefault(a => a.StartsWith(prefix));

            if (token is null)
                return null;

            return token.Replace(prefix, "");
        }

        private UserDto ExtractDataFromToken(string token)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken? jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken is null)
                throw new InvalidOperationException("Invalid token.");

            JwtPayload payload = jsonToken.Payload;

            DateTime createdAt;
            if (!DateTime.TryParse((string)payload["CreatedAt"], out createdAt))
            {
                createdAt = DateTime.MinValue;
            }
            
            return new UserDto()
            {
                Id = int.Parse((string)payload["Id"]),
                Username = (string)payload["Username"],
                Email = (string)payload["Email"],
                CreateAt = createdAt,
                Token = token,
            };
        }
    }
}