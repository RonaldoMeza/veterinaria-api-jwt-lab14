using Veterinaria.Entidades.DTOs;

namespace Veterinaria.Negocio.Servicios.Interfaces;

public interface IPropietarioServicio
{
    Task<List<PropietarioDto>> ListarAsync();
    Task<PropietarioDto?> ObtenerAsync(int id);
    Task<List<MascotaDto>?> MascotasAsync(int id);
    Task<PropietarioDto> CrearAsync(PropietarioCrearDto dto);
    Task<bool> ActualizarAsync(int id, PropietarioActualizarDto dto);
    Task<bool> EliminarAsync(int id);
}
