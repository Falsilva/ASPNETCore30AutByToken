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

        /**
         * Método Anonymous
         * Qualquer usuário pode acessar por esta rota
         * Mostra "Anônimo" na tela (response) 
         */
        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string Anonymous() => "Anônimo";

        /**
         * Método Authenticated
         * Anotação Authorize, restringe o acesso para a rota apenas por usuários autenticados
         * Mostra "Autenticado" na tela (response) com o nome do usuário obtido do token com User.Identity.Name,
         * por ter sido criado o Claim, com o ClaimTypes.Name, associado ao User.UserName, lá no TokenService
         */
        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() => string.Format("Autenticado - {0}", User.Identity.Name);

        /**
         * Método Employee
         * Anotação Authorize, restringe o acesso para a rota apenas por usuários autenticados 
         * e com os perfis employee ou manager
         * Mostra "Autenticado" na tela (response) 
         */
        [HttpGet]
        [Route("employee")]
        [Authorize(Roles = "employee, manager")]
        public string Employee() => "Funcionário";

        /**
         * Método Manager
         * Anotação Authorize, restringe o acesso para a rota apenas por usuários autenticados 
         * e com o perfil manager
         * Mostra "Autenticado" na tela (response) 
         */
        [HttpGet]
        [Route("manager")]
        [Authorize(Roles = "manager")]
        public string Manager() => "Gerente";
    }
}