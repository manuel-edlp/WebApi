﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Dtos;
using Microsoft.AspNetCore.Http;
using WebApi.Services;
using Microsoft.AspNetCore.JsonPatch;
using System.Diagnostics;
using Prometheus;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("WebApi/[controller]")]
    public class VideoJuegoController : ControllerBase
    {
        private readonly IVideoJuegoService _videoJuegoService;
        public VideoJuegoController(IVideoJuegoService videoJuegoService)
        {
            _videoJuegoService = videoJuegoService;
        }







        // Crear un contador para las solicitudes recibidas en tu endpoint
        private static readonly Counter solicitudesRecibidasContador = Metrics.CreateCounter(
            "solicitudes_recibidas", "Cantidad de solicitudes recibidas en el endpoint listar videojuegos");




        [HttpGet("listar")] // Lista Videojuegos
        public async Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegos(){

            solicitudesRecibidasContador.Inc();  // Incrementa el contador de solicitudes recibidas

            var resultados = await _videoJuegoService.GetAllVideoJuegos();

            return resultados;
        }










        // Crear un histograma para el tiempo de ejecución del endpoint
        private static readonly Histogram tiempoDeEjecucionHistograma = Metrics.CreateHistogram(
            "tiempo_ejecucion_endpoint", "Tiempo de ejecución del endpoint GetAllVideoJuegos");


        [HttpGet("listarNombres")] // Listar nombres de Videojuegos
        public async Task<IEnumerable<VideoJuegoNombreDto>> GetAllNombres()
        {
            var cronometro = Stopwatch.StartNew(); // Inicia el cronómetro

            var resultados = await _videoJuegoService.GetAllNombres();

            cronometro.Stop(); // Detiene el cronómetro

            var tiempoTranscurrido = cronometro.Elapsed.TotalSeconds; // Obtiene el tiempo transcurrido

            tiempoDeEjecucionHistograma.Observe(tiempoTranscurrido); // Registrar la duración en el histograma

            return resultados;
        }












        [HttpGet("buscar/{nombre}")] // Buscar Videojuego por nombre
        public async Task<IActionResult> GetVideoJuegoByNombre(string nombre)
        {
            var videojuego = await _videoJuegoService.GetVideoJuegoByNombre(nombre);
            if (videojuego == null)
            {
                return NotFound(new { message = $"El videojuego con nombre {nombre} no existe" });
            }
            else return Ok(videojuego);

        }


        [HttpGet("listar/{busqueda}")] // Listar nombres de videojuegos buscando por nombre
        public async Task<IEnumerable<VideoJuegoNombreDto>> BuscarNombresVideoJuegos(string busqueda) => await _videoJuegoService.BuscarNombresVideoJuegos(busqueda);
     

        [HttpGet("listarPorAño/{año}")] // Listar videojuegos por año
        public async Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegosAño(int año) => await _videoJuegoService.GetAllVideoJuegosAño(año);


        [HttpGet("listarPorDesarrollador/{desarrollador}")] // Listar videojuegos por desarrollador
        public async Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegosDesarrollador(string desarrollador) => await _videoJuegoService.GetAllVideoJuegosPeso(desarrollador);


        [HttpGet("listarPesoMenorIgual/{peso}")] // Listar videojuegos por peso menor o igual a un peso determinado
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
                return CreatedAtAction("GetVideoJuegoByNombre", new { nombre = videojuegodto.nombre }, videojuegodto);
            }
            else
            {
                // Retorno respuesta de fallo del servidor con el codigo 500
                return StatusCode(500, "Videojuego no creado, error interno del servidor.");
            }
        }


        [HttpDelete("{nombre}")] // elimina videojuego
        public async Task <IActionResult> EliminarVideoJuego(string nombre)
        {
            if (await _videoJuegoService.EliminarVideoJuego(nombre))
            {
                // Devuelvo una respuesta de éxito al eliminar
                return NoContent(); // Código 204 (Sin contenido)
            }
            else
            {
                // Si la eliminación falla o el videojuego no existe, devuelvo NotFound
                return NotFound($"Fallo al eliminar,videojuego con nombre {nombre} no encontrado");
            }
        }

        [HttpPut("{nombre}")] // modifico un videojuego completo por nombre
        public async Task <IActionResult> ModificarVideoJuego([FromBody] VideoJuegoDto videoJuegoNuevoDto, string nombre)
        {
            if (videoJuegoNuevoDto == null) // verifico que los datos no esten vacios
            {
                return BadRequest("Datos del videojuego inválidos");
            }

            if (await _videoJuegoService.ModificarVideoJuego(videoJuegoNuevoDto, nombre)) // verifico si se modifica exitosamente
            {
                var videoJuegoModificadoDto = await _videoJuegoService.GetVideoJuegoByNombre(videoJuegoNuevoDto.nombre);
                return Ok(videoJuegoModificadoDto);
            }
            else  return NotFound("Fallo en la modificación");
        }

        [HttpPatch("{nombre}")] // Modifica el nombre de un videojuego por ID
        public async Task<IActionResult> Modificar(string nombre, [FromBody] JsonPatchDocument<VideoJuegoDto> jsonPatch)
        {
            if (jsonPatch == null)
            {
                return BadRequest("Los datos no son válidos.");
            }

            if (await _videoJuegoService.Modificar(nombre, jsonPatch))
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
