using Veterinaria.Entidades.DTOs;

namespace Veterinaria.Negocio.Servicios.Interfaces;

public interface IMascotaServicio
{
    Task<List<MascotaDto>> ListarAsync(string? especie, string? nombre);
    Task<MascotaDto?> ObtenerAsync(int id);
    Task<MascotaDto> CrearAsync(MascotaCrearDto dto);
    Task<bool> ActualizarAsync(int id, MascotaActualizarDto dto);
    Task<bool> EliminarAsync(int id);
}
