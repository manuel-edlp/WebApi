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

    public class VideoJuegoService : IVideoJuegoService
    {
        private readonly ApiDb _context;
        private readonly IMapper _mapper;


        public VideoJuegoService(IConfiguration configuration, IMapper mapper,ApiDb context)
        {
            _context = context;

            _mapper = mapper;

        }


        public async Task<VideoJuegoDto> GetVideoJuegoByNombre(string nombre)
        {
            // Realiza la búsqueda de videojuego por nombre
            var videojuego = await _context.VideoJuego
               .Include(v => v.desarrollador)
               .FirstOrDefaultAsync(v => v.nombre.ToLower() == nombre.ToLower());
               
               

            var videoJuegoDto = _mapper.Map<VideoJuegoDto>(videojuego);

            return videoJuegoDto;
        }

        public async Task<IEnumerable<VideoJuegoNombreDto>> GetAllNombres()
        {
            // Realiza una consulta a la base de datos para devolver todos los videojuegos
            var videojuegos = await _context.VideoJuego.ToListAsync();

            var videoJuegosNombreDto = _mapper.Map<IEnumerable<VideoJuegoNombreDto>>(videojuegos);

            // Devuelve la lista de videojuegos
            return videoJuegosNombreDto;
        }

        public async Task<IEnumerable<VideoJuegoNombreDto>> BuscarNombresVideoJuegos(string busqueda)
        {
            // Realiza la búsqueda de videojuegos por nombre
            var videojuegos = await _context.VideoJuego
               .Where(v => v.nombre.ToLower().Contains(busqueda.ToLower()))
               .ToListAsync();

            var videoJuegosNombreDto = _mapper.Map<IEnumerable<VideoJuegoNombreDto>>(videojuegos);

            return videoJuegosNombreDto;

        }
        public async Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegos()
        {
            // Realiza una consulta a la base de datos para devolver todos los videojuegos
            var videojuegos = await _context.VideoJuego
                .Include(v => v.desarrollador)
                .ToListAsync();

            var videojuegosdto = _mapper.Map<IEnumerable<VideoJuegoDto>>(videojuegos);

            // Devuelve la lista de videojuegos
            return videojuegosdto;
        }

        public async Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegosAño(int año)
        {
            // Realiza una consulta a la base de datos para devolver todos los videojuegos de un año determinado
            var videojuegos = await _context.VideoJuego.Where(v => v.año == año)
                .Include(v => v.desarrollador)
                .ToListAsync();

            var videojuegosdto = _mapper.Map<IEnumerable<VideoJuegoDto>>(videojuegos);

            // Devuelve la lista de videojuegos del año determinado
            return videojuegosdto;
        }

        public async Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegosPeso(string desarrollador)
        {
            // Realizo una consulta a la base de datos para devolver todos los videojuegos de un desarrollador determinado
            var videojuegos = await _context.VideoJuego.Where(v => v.desarrollador.nombre.ToLower() == desarrollador.ToLower())
                .Include(v => v.desarrollador)
                .ToListAsync();

            var videojuegosdto = _mapper.Map<IEnumerable<VideoJuegoDto>>(videojuegos);

            // Devuelvo la lista de videojuegos del año determinado
            return videojuegosdto;
        }

        public async Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegosDesarrollador(float peso)
        {
            // Realizo una consulta a la base de datos para devolver todos los videojuegos de un año determinado
            var videojuegos = await _context.VideoJuego.Where(v => v.peso <= peso)
                .Include(v => v.desarrollador)
                .ToListAsync();

            var videojuegosdto = _mapper.Map<IEnumerable<VideoJuegoDto>>(videojuegos);

            // Devuelvo la lista de videojuegos del año determinado
            return videojuegosdto;
        }


        public async Task<bool> AgregarVideoJuego(VideoJuegoDto videojuegodto)
        {
            try
            {
                if (videojuegodto == null)
                {
                    return false;
                }

                // Verifico si el desarrollador ya existe en la base de datos
                Desarrollador desarrollador = await _context.Desarrollador
                    .FirstOrDefaultAsync(d => d.nombre == videojuegodto.desarrollador);

                if (desarrollador == null)
                {
                    // Si el desarrollador no existe cancelo el agregar
                    return false;
           
                }

                // Crea el VideoJuego y asigna el id del desarrollador
                var videojuego = _mapper.Map<VideoJuego>(videojuegodto);
                videojuego.desarrolladorId = desarrollador.desarrolladorId;

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

        public async Task<bool> EliminarVideoJuego(string nombre)
        {
            try
            {
                // Obtener el videojuego del contexto de la base de datos
                VideoJuego videojuego = await _context.VideoJuego
                 .FirstOrDefaultAsync(v => v.nombre == nombre);

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
        public async Task<bool> ModificarVideoJuego(VideoJuegoDto videoJuegoNuevoDto, string nombre)
        {
            try
            {
                var videojuego = _mapper.Map<VideoJuego>(videoJuegoNuevoDto);
                videojuego = await _context.VideoJuego
                    .Include(v => v.desarrollador)
                    .FirstOrDefaultAsync(v => v.nombre == nombre);

                if (videojuego == null) // verifico que se encuentre el videojuego
                {
                    return false;
                }

                var desarrollador = await _context.Desarrollador
                    .Where(d => d.nombre == videoJuegoNuevoDto.desarrollador)
                    .FirstOrDefaultAsync();

                if (desarrollador == null)
                {
                    return false;
                }

                videojuego.nombre = videoJuegoNuevoDto.nombre;
                videojuego.año = videoJuegoNuevoDto.año;
                videojuego.desarrolladorId = desarrollador.desarrolladorId;
                videojuego.peso = videoJuegoNuevoDto.peso;

                await _context.SaveChangesAsync();  // actualizo bd

                return true;    // retorno modificacion exitosa

            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> Modificar(string nombre, JsonPatchDocument<VideoJuegoDto> jsonPatch)
        {
            try
            {

                // Buscar el videojuego por ID
                var videojuego = await _context.VideoJuego.FirstOrDefaultAsync(v => v.nombre == nombre);

                // Si el videojuego no existe, devuelve false
                if (videojuego == null)
                {
                    return false;
                }
                var videojuegoDto = _mapper.Map<VideoJuegoDto>(videojuego);
                jsonPatch.ApplyTo(videojuegoDto);

                _mapper.Map(videojuegoDto, videojuego);

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
