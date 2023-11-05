using AutoMapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using WebApi.Models;

namespace WebApi.Services
{
    public class DesarrolladorService
    {
        private readonly ApiDb _context;
        private readonly IMapper _mapper;
        public DesarrolladorService(IConfiguration configuration, IMapper mapper, ApiDb context)
        {
            _context = context;

            _mapper = mapper;

        }
        public async Task<IEnumerable<DesarrolladorDto>> GetAllDesarrolladores()
        {
            // Realiza una consulta a la base de datos para devolver todos los Desarrolladores
            var desarrolladores = await _context.Desarrollador.ToListAsync();

            var desarrolladoresDto = _mapper.Map<IEnumerable<DesarrolladorDto>>(desarrolladores);

            // Devuelve la lista de Desarrolladores
            return desarrolladoresDto;
        }

        public async Task<DesarrolladorDto> GetDesarrolladorByNombre(string nombre)
        {
            // Realiza la búsqueda de desarollador por nombre
            var desarrollador = await _context.Desarrollador.FirstOrDefaultAsync(d => d.nombre.ToLower() == nombre.ToLower());

            var desarrolladorDto = _mapper.Map<DesarrolladorDto>(desarrollador);

            return desarrolladorDto;
        }

        public async Task<bool> AgregarDesarrollador(DesarrolladorDto desarrolladordto)
        {
            try
            {
                if (desarrolladordto == null)
                {
                    return false;
                }

                // Verifico si el desarrollador ya existe en la base de datos
                Desarrollador desarrollador = await _context.Desarrollador
                    .FirstOrDefaultAsync(d => d.nombre.ToLower() == desarrolladordto.nombre.ToLower());

                if (desarrollador == null) // Si el desarrollador no existe lo creo
                {
                    desarrollador = new Desarrollador { nombre = desarrolladordto.nombre };
                    _context.Desarrollador.Add(desarrollador);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false; // si ya existe no lo creo
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EliminarDesarrollador(string nombre)
        {
            try
            {
                // Obtener el Desarrollador del contexto de la base de datos
                Desarrollador desarrollador = await _context.Desarrollador
                 .FirstOrDefaultAsync(d => d.nombre == nombre);

                // Si el Desarrollador no existe, devuelve falso
                if (desarrollador == null)
                {
                    return false;
                }

                // Elimino el Desarrollador del contexto de la base de datos
                _context.Desarrollador.Remove(desarrollador);

                // Guardo los cambios en la base de datos
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ModificarDesarrollador(DesarrolladorDto desarrolladorNuevoDto, string nombre)
        {
            try
            {
                var desarrollador = await _context.Desarrollador.FirstOrDefaultAsync(d => d.nombre.ToLower() == nombre.ToLower());

                if (desarrollador == null) // verifico que se encuentre el desarrollador
                {
                    return false;
                }

                desarrollador.nombre = desarrolladorNuevoDto.nombre;

                await _context.SaveChangesAsync();  // actualizo bd

                return true;    // retorno modificacion exitosa
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
