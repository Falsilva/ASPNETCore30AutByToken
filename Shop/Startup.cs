using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Shop
{
    // Rodamos a nossa aplicação com o comando dotnet watch run, e então o localhost:5001 estará disponível
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Informando ao ASP.NET que a autenticação é via JWT
            // Adicionamos antes o CORs para não termos problemas de cors ao executarmos em localhost
            services.AddCors();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shop", Version = "v1" });
            });

            /** Aqui, de fato, informamos que temos uma autenticação JWT, configuração padrão já que é apenas para uma API e não várias
             * Caso for trabalhar com IdentityServer ou ter um login distribuídi, então será preciso configurar o Challenge, Issuer e vários outros itens
             * No AddJwtBearer, informamos que o formato do token é JWT
             */
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            services.AddAuthentication(x => 
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })            
            .AddJwtBearer(x => 
            {
                x.RequireHttpsMetadata = false;                                 // requirir https
                x.SaveToken = true;                                             // salvar o token
                x.TokenValidationParameters = new TokenValidationParameters()   // como valida o token
                {
                    ValidateIssuerSigningKey = true,                            // precisa validar a chave
                    IssuerSigningKey = new SymmetricSecurityKey(key),           // informamos que é uma chave simétrica
                    ValidateIssuer = false,                                     // não precisa validar o Issuer e nem o Audience, que são coisas quando estamos distribuindo a nossa aplicação, que remete a configuração do Challenge referido acima
                    ValidateAudience = false
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shop v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            /** Deve ter a chamada do UseAuthorization, caso não tenha, porém antes adicionamos o
             * UseCors para não termos problemas com Cors, 
             * permitindo todas as origens, métodos e cabeçalhos
             */
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            /** E então, depois de adicionamos o Cors, adicionamos o authentication e o authorization
             * pois vamos ver toda a parte de Roles aqui
             */
            app.UseAuthentication(); 
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
