using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using WebApi.Data;
using Npgsql.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApi.Controllers;
using AutoMapper;
using WebApi.Services;
using WebApi.Models;
using WebApi.Dtos;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<VideoJuego, VideoJuegoDto>()
                    .ForMember(dto => dto.desarrollador, opt => opt.MapFrom(src => src.desarrollador.nombre));
                CreateMap<VideoJuegoDto, VideoJuego>()
                    .ForMember(dest => dest.desarrollador, opt => opt.Ignore()); // Ignora la propiedad de navegación para evitar problemas de seguimiento de Entity Framework


            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApiDb>(options => options.UseNpgsql(Configuration.GetConnectionString("PostgreSQLConnection")));
            services.AddControllers().AddNewtonsoftJson();
            
            // registro el servicio videojuegos para poder inyectarlo en mi videojuego controller
            services.AddScoped<VideoJuegoService>();
      

            // Configuración de AutoMapper
            services.AddAutoMapper(typeof(Startup));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
                // interfaz grafica de swagger: https://localhost:5001/swagger/index.html
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
