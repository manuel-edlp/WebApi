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
    }
}
/* 
 comando para migraciones: 
 dotnet ef migrations add migration
 dotnet ef database update  

*/
