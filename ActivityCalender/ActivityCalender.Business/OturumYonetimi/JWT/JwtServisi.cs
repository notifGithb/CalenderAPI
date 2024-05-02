using ActivityCalender.Business.OturumYonetimi.JWT.Token;
using ActivityCalender.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ActivityCalender.Business.OturumYonetimi.JWT
{
    public class JwtServisi : IJwtServisi
    {
        private readonly IConfiguration _configuration;

        public JwtServisi(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtToken JwtTokenOlustur(Kullanici kullanici)
        {
            var jwtToken = new JwtToken();
            var tokenhandler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey key =
                new(Encoding.UTF8.GetBytes(_configuration["JWT:Key"] ?? string.Empty));

            jwtToken.AccessTokenTime = DateTime.UtcNow.AddHours(1);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, kullanici.Id)
            };
            var tokendesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Audience = _configuration["Jwt:Audience"],
                Issuer = _configuration["Jwt:Issuer"],
                Expires = jwtToken.AccessTokenTime,
                SigningCredentials = new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenhandler.CreateToken(tokendesc);
            var finaltoken = tokenhandler.WriteToken(token);
            return new JwtToken()
            {
                AccessToken = finaltoken,
                AccessTokenTime = jwtToken.AccessTokenTime
            };
        }
    }
}
