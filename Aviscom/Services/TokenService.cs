using Aviscom.Models.Usuario;
using Aviscom.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Aviscom.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(UsuarioPessoaFisica usuario, List<string> funcoes)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Pega a chave secreta do appsettings
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]!);

            // Define os "Claims" (informações dentro do token)
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()), // ID do usuário
                new Claim(JwtRegisteredClaimNames.Name, usuario.Nome), // Nome do usuário
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // ID único para o token
            };

            // Adiciona as "Funções" (Roles) como claims
            foreach (var funcao in funcoes)
            {
                claims.Add(new Claim(ClaimTypes.Role, funcao));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8), // Duração do token
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}