using AutoMapper;
using Veterinaria.Datos.Repositorios.Interfaces;
using Veterinaria.Entidades.DTOs;
using Veterinaria.Entidades.Entidades;
using Veterinaria.Negocio.Servicios.Interfaces;

namespace Veterinaria.Negocio.Servicios;

public class MascotaServicio(IMascotaRepositorio mascotas, IMapper mapper) : IMascotaServicio
{
    public async Task<List<MascotaDto>> ListarAsync(string? especie, string? nombre) => mapper.Map<List<MascotaDto>>(await mascotas.ListarAsync(especie, nombre));

    public async Task<MascotaDto?> ObtenerAsync(int id)
    {
        var mascota = await mascotas.ObtenerAsync(id);
        return mascota is null ? null : mapper.Map<MascotaDto>(mascota);
    }

    public async Task<MascotaDto> CrearAsync(MascotaCrearDto dto)
    {
        await ValidarAsync(dto);
        var mascota = mapper.Map<Mascota>(dto);
        await mascotas.AgregarAsync(mascota);
        await mascotas.GuardarAsync();
        return mapper.Map<MascotaDto>(mascota);
    }

    public async Task<bool> ActualizarAsync(int id, MascotaActualizarDto dto)
    {
        if (id != dto.IdMascota) throw new ArgumentException("El identificador no coincide.");
        var mascota = await mascotas.ObtenerAsync(id);
        if (mascota is null) return false;
        await ValidarAsync(dto);
        mapper.Map(dto, mascota);
        await mascotas.GuardarAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var mascota = await mascotas.ObtenerAsync(id);
        if (mascota is null) return false;
        mascotas.Eliminar(mascota);
        await mascotas.GuardarAsync();
        return true;
    }

    private async Task ValidarAsync(MascotaCrearDto dto)
    {
        if (dto.FechaNacimiento > DateOnly.FromDateTime(DateTime.Today)) throw new ArgumentException("La fecha no puede ser futura.");
        if (!await mascotas.PropietarioExisteAsync(dto.IdPropietario)) throw new ArgumentException("El propietario no existe.");
    }
}
