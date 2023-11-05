using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Models;
using WebApi.Services;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("WebApi/[controller]")]
    public class DesarrolladorController : ControllerBase
    {
        private readonly DesarrolladorService _desarrolladorService;
        public DesarrolladorController(DesarrolladorService desarrolladorService)
        {
            _desarrolladorService = desarrolladorService;
        }

        [HttpGet("listar")] // Listar Desarrolladores
        public async Task<IEnumerable<DesarrolladorDto>> GetAllDesarrolladores()
        {
            var resultados = await _desarrolladorService.GetAllDesarrolladores();
            return resultados;
        }

        [HttpGet("buscar/{nombre}")] // Buscar desarrollador por nombre
        public async Task<IActionResult> GetDesarrolladorByNombre(string nombre)
        {
            var desarrollador = await _desarrolladorService.GetDesarrolladorByNombre(nombre);
            if (desarrollador == null)
            {
                return NotFound(new { message = $"El desarrollador con nombre {nombre} no existe" });
            }
            else return Ok(desarrollador);

        }

        [HttpPost] // agrega Desarrollador
        //[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(VideoJuego))]
        public async Task<IActionResult> AgregarVideoJuego([FromBody] DesarrolladorDto desarrolladordto)
        {
            if (desarrolladordto == null)
            {
                return BadRequest("Los datos del desarrollador no son válidos.");
            }

            if (await _desarrolladorService.AgregarDesarrollador(desarrolladordto))
            {
                // Devuelvo una respuesta de éxito con el código de estado 201 (Created)
                return CreatedAtAction("GetDesarrolladorByNombre", new { nombre = desarrolladordto.nombre }, desarrolladordto);
            }
            else
            {
                // Retorno respuesta de fallo del servidor con el codigo 500
                return StatusCode(500, "desarrollador no creado, error interno del servidor.");
            }
        }

        [HttpDelete("{nombre}")] // elimina Desarrollador
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(void), StatusCode = 204)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string), StatusCode = 404)]
        public async Task<IActionResult> EliminarDesarrollador(string nombre)
        {
            if (await _desarrolladorService.EliminarDesarrollador(nombre))
            {
                // Devuelvo una respuesta de éxito al eliminar
                return NoContent(); // Código 204 (Sin contenido)
            }
            else
            {
                // Si la eliminación falla o el videojuego no existe, devuelvo NotFound
                return NotFound($"Fallo al eliminar desarrollador con nombre {nombre}");
            }
        }
    }
}
