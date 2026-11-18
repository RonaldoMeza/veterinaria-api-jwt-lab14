using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entidades.DTOs;

public class PropietarioActualizarDto : PropietarioCrearDto
{
    [Range(1, int.MaxValue)]
    public int IdPropietario { get; set; }
}
