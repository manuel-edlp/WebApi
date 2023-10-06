using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Dtos;
using WebApi.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace WebApi.Services
{
    
    public class VideoJuegoService
    {
        private readonly ApiDb _context;
        private readonly IMapper _mapper;


        public VideoJuegoService(IConfiguration configuration, IMapper mapper)
        {
            var connectionString = configuration.GetConnectionString("PostgreSQLConnection");

            var optionsBuilder = new DbContextOptionsBuilder<ApiDb>();

            optionsBuilder.UseNpgsql(connectionString);

            _context = new ApiDb(optionsBuilder.Options);

            _mapper = mapper;

        }


        public string GetNombre(int id)  // falta manejar que retorna si no lo encuentra
        {

            // Obtén el videojuego de la base de datos
            var videojuego = _context.VideoJuego.FirstOrDefault(v => v.id == id);

            var videojuegodto = _mapper.Map<VideoJuegoDto>(videojuego);

            // Devuelve el nombre del videojuego
            return videojuegodto.nombre;
        }

        public async Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegos()
        {
            // Realiza una consulta a la base de datos para devolver todos los videojuegos
            var videojuegos = await _context.VideoJuego.ToListAsync();

            var videojuegosdto = _mapper.Map<List<VideoJuegoDto>>(videojuegos);

            // Devuelve la lista de videojuegos
            return videojuegosdto;
        }

        public async Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegosAño(int año)
        {
            // Realiza una consulta a la base de datos para devolver todos los videojuegos de un año determinado
            var videojuegos = await _context.VideoJuego.Where(v => v.año == año).ToListAsync();

            var videojuegosdto = _mapper.Map<List<VideoJuegoDto>>(videojuegos);

            // Devuelve la lista de videojuegos del año determinado
            return videojuegosdto;
        }

        public async Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegosDesarrollador(string desarrollador)
        {
            // Realiza una consulta a la base de datos para devolver todos los videojuegos de un desarrollador determinado
            var videojuegos = await _context.VideoJuego.Where(v => v.desarrollador.nombre == desarrollador).ToListAsync();

            var videojuegosdto = _mapper.Map<List<VideoJuegoDto>>(videojuegos);

            // Devuelve la lista de videojuegos del año determinado
            return videojuegosdto;
        }

        public async Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegosDesarrollador(float peso)
        {
            // Realiza una consulta a la base de datos para devolver todos los videojuegos de un año determinado
            var videojuegos = await _context.VideoJuego.Where(v => v.peso <= peso).ToListAsync();

            var videojuegosdto = _mapper.Map<List<VideoJuegoDto>>(videojuegos);

            // Devuelve la lista de videojuegos del año determinado
            return videojuegosdto;
        }


        public async Task<bool> AgregarVideoJuego(VideoJuego videojuego)
        {
            try
            {
                if (videojuego == null)
                {
                    return false;
                }

                // Agregar el videojuego al contexto de la base de datos
                _context.VideoJuego.Add(videojuego);

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EliminarVideoJuego(int id)
        {
            try
            {
                // Obtener el videojuego del contexto de la base de datos
                VideoJuego videojuego = await _context.VideoJuego.FindAsync(id);

                // Si el videojuego no existe, devuelve falso
                if (videojuego == null)
                {
                    return false;
                }

                // Elimino el videojuego del contexto de la base de datos
                _context.VideoJuego.Remove(videojuego);

                // Guardo los cambios en la base de datos
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> ModificarVideoJuego(VideoJuego videoJuegoNuevo,int id)
        {
            try
            {
                VideoJuego videojuego = await _context.VideoJuego.FindAsync(id);
                if (videojuego == null) // verifico que se encuentre el videojuego
                {
                    return false;
                }

                videojuego.nombre = videoJuegoNuevo.nombre;
                videojuego.año = videoJuegoNuevo.año;
                videojuego.desarrollador = videoJuegoNuevo.desarrollador;
                videojuego.peso = videoJuegoNuevo.peso;

                await _context.SaveChangesAsync();  // actualizo bd

                return true;    // retorno modificacion exitosa

            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> Modificar(int id, JsonPatchDocument<VideoJuego> jsonPatch)
        {
            try
            {
                // Buscar el videojuego por ID
                var videojuego = await _context.VideoJuego.FindAsync(id);

                // Si el videojuego no existe, devuelve false
                if (videojuego == null)
                {
                    return false;
                }
                jsonPatch.ApplyTo(videojuego);
                await _context.SaveChangesAsync();  // actualizo bd
                return true;

            }
            catch (Exception)
            {
                // Si ocurre un error, devuelve false
                return false;
            }
        }
    }
}
