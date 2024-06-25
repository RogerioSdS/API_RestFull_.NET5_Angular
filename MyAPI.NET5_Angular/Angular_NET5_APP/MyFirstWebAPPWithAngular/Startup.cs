using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProEventos.Application;
using ProEventos.Application.Contratos;
using ProEventos.Persistence;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Contratos;
using AutoMapper;
using System;
using System.Text.Json.Serialization;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ProEventos.Domain.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MyFirstWebAPPWithAngular
{
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
            services.AddDbContext<ProEventosContext>(
                context => context.UseSqlite(Configuration.GetConnectionString("Default"))
            );

            // Configura as opções de senha padrão do Identity, como: quantidade mínima de caracteres, exigir letras maiúsculas, letras minúsculas, números e caracteres especiais.
            services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
            })
                // Configura as classes para que o Identity possa gerenciar as funções de usuário (RoleManager), gerenciar o login (SignInManager) e possua um tipo de função de usuário definido (Role).
                // Configura o gerenciamento de roles (funções) de usuário.
                .AddRoles<Role>()
                // Adiciona o RoleManager para gerenciar as funções no sistema.
                .AddRoleManager<RoleManager<Role>>()
                // Adiciona o SignInManager para gerenciar o processo de login de usuários.
                .AddSignInManager<SignInManager<User>>()
                // Adiciona o RoleValidator para validar as regras e restrições definidas para as funções.
                .AddRoleValidator<RoleValidator<Role>>()
                // Configura o uso do Entity Framework para armazenar informações de identidade no banco de dados usando o ProEventosContext.
                .AddEntityFrameworkStores<ProEventosContext>()
                // Adiciona provedores de token padrão para funcionalidades como recuperação de senha e verificação de email.
                .AddDefaultTokenProviders();

            // Configura a autenticação usando o Bearer Token com o padrão JWT (JSON Web Token).
            // Especifica as opções de validação do token, como a chave de assinatura, se deve validar o emissor e o receptor.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Indica se deve validar a chave de assinatura do token.
                        ValidateIssuerSigningKey = true,
                        // A chave de assinatura usada para validar o token.
                        // A chave é recuperada do arquivo de configuração do aplicativo.
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"])),
                        // Indica se deve validar o emissor do token.
                        // Neste caso, não é necessário validar o emissor.
                        ValidateIssuer = false,
                        // Indica se deve validar o receptor do token.
                        // Neste caso, também não é necessário validar o receptor.
                        ValidateAudience = false
                    };
                });
            services.AddControllers()
            // Configura a serialização JSON para converter enums para strings usando o StringEnumConverter .
                .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
                // Ignora os loops que contem nas classes
                //onde evento chama palestrante, que chama evento e assim por diante, criando um looping infinito
                .AddNewtonsoftJson(options => 
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            
            // Configura o serviço do AutoMapper adicionando todas as classes de mapeamento no aplicativo.
            // Isso é feito chamando o método AddAutoMapper no interface IServiceCollection,
            // passando em todos os assemblies do aplicativo. O método AddAutoMapper pesquisa
            // os assemblies para classes que implementam a interface IMapping. Essas classes
            // definem os mapeamentos entre diferentes objetos do aplicativo.
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //esse metodo foi aplicado no Helpers/ProEventosProfile.cs
            
            services.AddScoped<IEventosService, EventosService>();
            services.AddScoped<ILoteService, LoteService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAccountService, AccountService>();
            
            services.AddScoped<IGeralPersist, GeralPersist>();
            services.AddScoped<IEventoPersist, EventosPersist>();
            services.AddScoped<ILotePersist, LotePersist>();
            services.AddScoped<IUserPersist, UserPersist>();
           
            services.AddCors();
           
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My First API With Angular", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My First API With Angular v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            /* UseCors = Permite que qualquer origem (localhost:4200, localhost:3000, etc) realize 
            qualquer tipo de requisição (GET, POST, PUT, DELETE, etc) e qualquer header 
            (Content-Type, Authorization, etc)
            */
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseStaticFiles(new StaticFileOptions(){
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Resources")),
                RequestPath = new PathString("/Resources")
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
