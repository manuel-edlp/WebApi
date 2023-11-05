using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Data
{
    public class ApiDb : DbContext
    {
        public ApiDb(DbContextOptions<ApiDb> options) : base(options)
        {

        }

        public DbSet<VideoJuego> VideoJuego => Set<VideoJuego>();
        public DbSet<Desarrollador> Desarrollador => Set<Desarrollador>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VideoJuego>()
                .HasOne(v => v.desarrollador)
                .WithMany()
                .HasForeignKey(v => v.desarrolladorId)
                .OnDelete(DeleteBehavior.NoAction); // Cambiar DeleteBehavior según tus requisitos
                
            base.OnModelCreating(modelBuilder); // Ejecutar otras configuraciones predeterminadas
        }
    }
}
/* 
 comando para migraciones: 
 dotnet ef migrations add NameMigration
 dotnet ef database update  

*/
