namespace Shop
{
    public static class Settings
    {
        // Para o token precisamos de uma chave com um hash (é a nossa chave privada que apenas o servidor tem)
        // Essa chave deve ficar no arquivo appsettings.json, para isso vai ser preciso usar Configuration e outras
        // implementações que não são abordadas neste tutorial, sendo assim a nossa chave é colocada aqui
        public static string Secret = "fedaf7d8863b48e197b9287d492b708e";   // Hash gerado aleatoriamente
    }
}