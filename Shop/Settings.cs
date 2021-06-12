namespace Shop
{
    /** Instale os pacotes:
     *
     *  1. Microsoft.AspNetCoreAuthentication: 
     *  Possui tudo o que é preciso para trabalhar com autenticação e autorização dentro do ASP.NET. 
     *  Comando dotnet add package Microsoft.AspNetCoreAuthentication
     *  
     *  2. Microsoft.AspNetCore.Authentication.JwtBearer
     *  Possui o mesmo que o pacote anterior, este tem o formato do TokenJWT que a gente trabalha
     *  Comando dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
     */

    public static class Settings
    {
        // Para o token precisamos de uma chave com um hash (é a nossa chave privada que apenas o servidor tem)
        // Essa chave deve ficar no arquivo appsettings.json, para isso vai ser preciso usar Configuration e outras
        // implementações que não são abordadas neste tutorial, sendo assim a nossa chave é colocada aqui
        public static string Secret = "fedaf7d8863b48e197b9287d492b708e";   // Hash gerado aleatoriamente
    }
}