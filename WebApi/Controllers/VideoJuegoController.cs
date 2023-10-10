using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Dtos;
using Microsoft.AspNetCore.Http;
using WebApi.Services;
using Microsoft.AspNetCore.JsonPatch;

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
        public async Task<IActionResult> GetNombre(int id) 
        {
            var nombre = await _videoJuegoService.GetNombre(id);
            if (nombre == "null")
            {
                return NotFound(new { message = $"El videojuego con id {id} no existe"});
            }
            else return Ok(nombre);
            
        } 


        [HttpGet("año/{año}")] // Listar videojuegos por año
        public async Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegosAño(int año) => await _videoJuegoService.GetAllVideoJuegosAño(año);


        [HttpGet("desarrollador/{desarrollador}")] // Listar videojuegos por desarrollador
        public async Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegosDesarrollador(string desarrollador) => await _videoJuegoService.GetAllVideoJuegosPeso(desarrollador);


        [HttpGet("peso/{peso}")] // Listar videojuegos por por peso menor o igual a un peso determinado
        public async Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegosPeso(float peso) => await _videoJuegoService.GetAllVideoJuegosDesarrollador(peso);


        [HttpPost] // agrega videojuego
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(VideoJuego))]
        public async Task<IActionResult> AgregarVideoJuego([FromBody] VideoJuegoDto videojuegodto)
        {
            if (videojuegodto == null)
            {
                return BadRequest("Los datos del videojuego no son válidos.");
            }

            if (await _videoJuegoService.AgregarVideoJuego(videojuegodto))
            {
                // Devuelvo una respuesta de éxito con el código de estado 201 (Created)
                return CreatedAtAction("GetNombre", new { id = videojuegodto.nombre }, videojuegodto);
            }
            else
            {
                // Retorno respuesta de fallo del servidor con el codigo 500
                return StatusCode(500, "Videojuego no creado, error interno del servidor.");
            }
        }


        [HttpDelete("{id}")] // elimina videojuego
        public async Task <IActionResult> EliminarVideoJuego(int id)
        {
            if (await _videoJuegoService.EliminarVideoJuego(id))
            {
                // Devuelvo una respuesta de éxito al eliminar
                return NoContent(); // Código 204 (Sin contenido)
            }
            else
            {
                // Si la eliminación falla o el videojuego no existe, devuelvo NotFound
                return NotFound($"Fallo al eliminar,videojuego con id {id} no encontrado");
            }
        }

        [HttpPut("{id}")] // modifico un videojuego completo por id
        public async Task <IActionResult> ModificarVideoJuego([FromBody] VideoJuegoDto videoJuegoNuevoDto, int id)
        {
            if (videoJuegoNuevoDto == null) // verifico que los datos no esten vacios
            {
                return BadRequest("Datos del videojuego inválidos");
            }

            if (await _videoJuegoService.ModificarVideoJuego(videoJuegoNuevoDto, id)) // verifico si se modifica exitosamente
            {
                return Ok(); // retorno codigo 200 por modificacion exitosa
            }
            else  return NotFound("Fallo en la modificación");
        }

        [HttpPatch("{id}")] // Modifica el nombre de un videojuego por ID
        public async Task<IActionResult> Modificar(int id, [FromBody] JsonPatchDocument<VideoJuegoDto> jsonPatch)
        {
            if (jsonPatch == null)
            {
                return BadRequest("Los datos no son válidos.");
            }

            if (await _videoJuegoService.Modificar(id, jsonPatch))
            {
                // Devuelvo una respuesta de éxito con código 200 (OK)
                return Ok();
            }
            else
            {
                // Si la modificación falla o el videojuego no existe, devuelvo NotFound
                return NotFound("Fallo en la modificacion.");
            }
        }
    }
}
