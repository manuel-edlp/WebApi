using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Dtos;

namespace WebApi.Services
{
    public interface IVideoJuegoService
    {
        Task<bool> AgregarVideoJuego(VideoJuegoDto videojuegodto);
        Task<bool> EliminarVideoJuego(string nombre);
        Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegos();
        Task<IEnumerable<VideoJuegoNombreDto>> GetAllNombres();
        Task<IEnumerable<VideoJuegoNombreDto>> BuscarVideoJuegos(string busqueda);
        Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegosAño(int año);
        Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegosDesarrollador(float peso);
        Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegosPeso(string desarrollador);
        Task<string> GetNombreById(int id);
        Task<bool> Modificar(int id, JsonPatchDocument<VideoJuegoDto> jsonPatch);
        Task<bool> ModificarVideoJuego(VideoJuegoDto videoJuegoNuevoDto, int id);
    }
}