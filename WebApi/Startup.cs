using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApi.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.Services;
using WebApi.Models;
using WebApi.Dtos;
using Prometheus;
using Microsoft.OpenApi.Models;

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
            
            // registro los servicios para poder inyectarlo en mis controller
            services.AddScoped<IVideoJuegoService,VideoJuegoService>();
            services.AddScoped<DesarrolladorService>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                policy => policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            });


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

            
            app.UseCors("AllowAllOrigins");

            app.UseHttpsRedirection();

            app.UseHttpMetrics(); // captura métricas relacionadas con las solicitudes HTTP

            app.UseRouting();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics(); // habilita un punto de acceso para la recopilación y exposición de todas las métricas generadas
            });

            
        }
    }
}
