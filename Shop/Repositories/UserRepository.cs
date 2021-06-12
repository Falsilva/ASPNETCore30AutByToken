using System.Collections.Generic;
using System.Linq;
using Shop.Models;

namespace Shop.Repositories
{
    /**
     * Essa classe simula o acesso a dados de usuário
     */
    public class UserRepository
    {
        public static User Get(string username, string password){
            var users = new List<User>();
            users.Add(new User { Id = 1, UserName = "batman", Password = "batman", Role = "manager" });
            users.Add(new User { Id = 2, UserName = "robin", Password = "batman", Role = "employee" });
            return users.Where(user => user.UserName.ToLower() == username.ToLower()).FirstOrDefault();
        }
    }
}