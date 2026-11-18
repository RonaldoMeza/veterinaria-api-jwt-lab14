using AutoMapper;
using Veterinaria.Datos.Repositorios.Interfaces;
using Veterinaria.Entidades.DTOs;
using Veterinaria.Entidades.Entidades;
using Veterinaria.Negocio.Excepciones;
using Veterinaria.Negocio.Servicios.Interfaces;

namespace Veterinaria.Negocio.Servicios;

public class PropietarioServicio(IPropietarioRepositorio propietarios, IMascotaRepositorio mascotas, IMapper mapper) : IPropietarioServicio
{
    public async Task<List<PropietarioDto>> ListarAsync() => mapper.Map<List<PropietarioDto>>(await propietarios.ListarAsync());

    public async Task<PropietarioDto?> ObtenerAsync(int id)
    {
        var propietario = await propietarios.ObtenerAsync(id);
        return propietario is null ? null : mapper.Map<PropietarioDto>(propietario);
    }

    public async Task<List<MascotaDto>?> MascotasAsync(int id)
    {
        if (await propietarios.ObtenerAsync(id) is null) return null;
        return mapper.Map<List<MascotaDto>>(await mascotas.PorPropietarioAsync(id));
    }

    public async Task<PropietarioDto> CrearAsync(PropietarioCrearDto dto)
    {
        dto.Dni = dto.Dni.Trim();
        if (await propietarios.ExisteDniAsync(dto.Dni)) throw new ReglaNegocioException("Ya existe un propietario con ese DNI.");
        var propietario = mapper.Map<Propietario>(dto);
        await propietarios.AgregarAsync(propietario);
        await propietarios.GuardarAsync();
        return mapper.Map<PropietarioDto>(propietario);
    }

    public async Task<bool> ActualizarAsync(int id, PropietarioActualizarDto dto)
    {
        if (id != dto.IdPropietario) throw new ArgumentException("El identificador no coincide.");
        var propietario = await propietarios.ObtenerAsync(id);
        if (propietario is null) return false;
        dto.Dni = dto.Dni.Trim();
        if (await propietarios.ExisteDniAsync(dto.Dni, id)) throw new ReglaNegocioException("Ya existe un propietario con ese DNI.");
        mapper.Map(dto, propietario);
        await propietarios.GuardarAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var propietario = await propietarios.ObtenerAsync(id);
        if (propietario is null) return false;
        if (await propietarios.TieneMascotasAsync(id)) throw new ReglaNegocioException("No se puede eliminar un propietario con mascotas.");
        propietarios.Eliminar(propietario);
        await propietarios.GuardarAsync();
        return true;
    }
}
