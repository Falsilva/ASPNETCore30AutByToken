using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Shop.Models;

namespace Shop.Services
{
    public class TokenService
    {
        public static string GenerateToken(User user) {
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);

            /** Gera o token, passando o tokenDescriptor, que são as informações passadas para que se gere o token
             * O token descriptor é dividido em três partes: subject, expires e signing credentials
             * Subject, são as Claims que ficam disponíveis para os controllers
             * Expires, quanto menor o tempo para expiração do token, melhor por questões de segurança
             * SigningCredentials, possui dois parâmetros, a chave a ser encriptada e o tipo de encriptação a ser utilizado
             * para o nosso caso, passamos uma chave simétrica com o tipo de criptografia SHA-256, muito seguro até o momento
             */
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = System.DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);  // gera o token
            return tokenHandler.WriteToken(token);  // converte para string
        }
    }
}