using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Repositories;
using Shop.Services;

namespace Shop.Controllers
{
    // Rota inicial para o controller, tudo que quisermos acessar dentro da nossa aplicação passa por esta rota
    [Route("v1/account")]
    public class HomeController : ControllerBase
    {
        /** 
         * Método Login
         * Método http POST, rota login, permite acesso anônimo, pois ainda não foi feito o login
         * Retorna um Task dynamic, pois em um momento retorna um NotFound e em outro retorna um usuário token
         * Nomeamos o método como Authenticate, e ele fica com um sublinhado warning, 
         * pois o método é assíncrono e não estamos usando o await em nenhum lugar, 
         * até mesmo porque o nosso UserRepository é estático, mas isso não tem problema
         */
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model)
        {
            // Busca o usuário no repositório (simulamos o banco) pelo nome e senha
            var user = UserRepository.Get(model.UserName, model.Password);

            // Checa se o usuário foi encontrado
            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos"});
            
            // Gera o token
            var token = TokenService.GenerateToken(user);
            
            // Escondendo a senha do usuário
            user.Password = ""; 
            
            // retorna o usuário e o token - retornamos o usuário para o Front-End não precisar fazer um novo get
            return new {
                user = user,
                token = token
            };
        }
    }
}