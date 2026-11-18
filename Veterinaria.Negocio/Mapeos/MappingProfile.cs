using AutoMapper;
using Veterinaria.Entidades.DTOs;
using Veterinaria.Entidades.Entidades;

namespace Veterinaria.Negocio.Mapeos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Propietario, PropietarioDto>();
        CreateMap<PropietarioCrearDto, Propietario>();
        CreateMap<PropietarioActualizarDto, Propietario>();

        CreateMap<Mascota, MascotaDto>()
            .ForMember(destino => destino.NombrePropietario,
                opcion => opcion.MapFrom(origen => origen.Propietario == null
                    ? string.Empty
                    : origen.Propietario.Nombres + " " + origen.Propietario.Apellidos));
        CreateMap<MascotaCrearDto, Mascota>();
        CreateMap<MascotaActualizarDto, Mascota>();
    }
}
