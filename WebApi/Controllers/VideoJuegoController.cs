using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Dtos;
using Microsoft.AspNetCore.Http;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("WebApi/[controller]")]
    public class VideoJuegoController : ControllerBase
    {
        private readonly VideoJuegoService _videoJuegoService;
        public VideoJuegoController(VideoJuegoService videoJuegoService)
        {
            _videoJuegoService = videoJuegoService;
        }

        [HttpGet] // Listar Videojuegos
        public async Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegos() => await _videoJuegoService.GetAllVideoJuegos();


        [HttpGet("{id}")] // Buscar Videojuego por id
        public string GetNombre(int id) => _videoJuegoService.GetNombre(id);


        [HttpGet("año/{año}")] // Listar videojuegos por año
        public async Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegosAño(int año) => await _videoJuegoService.GetAllVideoJuegosAño(año);


        [HttpGet("desarrollador/{desarrollador}")] // Listar videojuegos por desarrollador
        public async Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegosDesarrollador(string desarrollador) => await _videoJuegoService.GetAllVideoJuegosDesarrollador(desarrollador);


        [HttpGet("peso/{peso}")] // Listar videojuegos por por peso menor o igual a un peso determinado
        public async Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegosDesarrollador(float peso) => await _videoJuegoService.GetAllVideoJuegosDesarrollador(peso);


        [HttpPost] // agrega videojuego
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(VideoJuego))]
        public IActionResult AgregarVideoJuego([FromBody] VideoJuego videojuego)
        {
            if (videojuego == null)
            {
                return BadRequest("Los datos del videojuego no son válidos.");
            }

            if (_videoJuegoService.AgregarVideoJuego(videojuego))
            {
                // Devuelvo una respuesta de éxito con el código de estado 201 (Created)
                return CreatedAtAction("GetNombre", new { id = videojuego.id }, videojuego);
            }
            else
            {
                // Retorno respuesta de fallo del servidor con el codigo 500
                return StatusCode(500, "Error interno del servidor.");
            }
        }


        [HttpDelete("{id}")] // elimina videojuego
        public IActionResult EliminarVideoJuego(int id)
        {
            if (_videoJuegoService.EliminarVideoJuego(id))
            {
                // Devuelvo una respuesta de éxito
                return NoContent(); // Código 204 (Sin contenido)
            }
            else
            {
                // Si la eliminación falla o el videojuego no existe, devuelvo NotFound
                return NotFound();
            }
        }

        [HttpPut("{id}")] // modifico un videojuego completo por id
        public IActionResult ModificarVideoJuego([FromBody] VideoJuego videoJuegoNuevo, int id)
        {
            if (videoJuegoNuevo == null) // verifico que los datos no esten vacios
            {
                return BadRequest("Datos del videojuego inválidos");
            }

            if (_videoJuegoService.ModificarVideoJuego(videoJuegoNuevo, id)) // verifico si se modifica exitosamente
            {
                return Ok(); // retorno codigo 200 por modificacion exitosa
            }
            else  return NotFound("Fallo en la modificación");
        }

        [HttpPut("{id}/nombre")] // Modifica el nombre de un videojuego por ID
        public IActionResult ModificarNombreVideoJuego(int id, [FromBody] string nuevoNombre)
        {
            if (string.IsNullOrEmpty(nuevoNombre))
            {
                return BadRequest("El nuevo nombre no es válido.");
            }

            if (_videoJuegoService.ModificarNombre(id, nuevoNombre))
            {
                // Devuelvo una respuesta de éxito con código 200 (OK)
                return Ok();
            }
            else
            {
                // Si la modificación falla o el videojuego no existe, devuelve NotFound
                return NotFound("El videojuego no se encuentra en el sistema.");
            }
        }
    }
}
