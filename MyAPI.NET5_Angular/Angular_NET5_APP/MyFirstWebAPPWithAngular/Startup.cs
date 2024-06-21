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
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;

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
            services.AddControllers()
                .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);// Ignora os loops que contem nas classes
                //onde evento chama palestrante, que chama evento e assim por diante, criando um looping infinito
            
            // Configura o serviço do AutoMapper adicionando todas as classes de mapeamento no aplicativo.
            // Isso é feito chamando o método AddAutoMapper no interface IServiceCollection,
            // passando em todos os assemblies do aplicativo. O método AddAutoMapper pesquisa
            // os assemblies para classes que implementam a interface IMapping. Essas classes
            // definem os mapeamentos entre diferentes objetos do aplicativo.
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //esse metodo foi aplicado no Helpers/ProEventosProfile.cs
            
            services.AddScoped<IEventosService, EventosService>();
            services.AddScoped<ILoteService, LoteService>();
            
            services.AddScoped<IEventoPersist, EventosPersist>();
            services.AddScoped<IGeralPersist, GeralPersist>();
            services.AddScoped<ILotePersist, LotePersist>();
           
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
