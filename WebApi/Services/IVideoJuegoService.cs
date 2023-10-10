﻿using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Dtos;

namespace WebApi.Services
{
    public interface IVideoJuegoService
    {
        Task<bool> AgregarVideoJuego(VideoJuegoDto videojuegodto);
        Task<bool> EliminarVideoJuego(int id);
        Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegos();
        Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegosAño(int año);
        Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegosDesarrollador(float peso);
        Task<IEnumerable<VideoJuegoDto>> GetAllVideoJuegosPeso(string desarrollador);
        Task<string> GetNombre(int id);
        Task<bool> Modificar(int id, JsonPatchDocument<VideoJuegoDto> jsonPatch);
        Task<bool> ModificarVideoJuego(VideoJuegoDto videoJuegoNuevoDto, int id);
    }
}