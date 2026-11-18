using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entidades.DTOs;

public class MascotaActualizarDto : MascotaCrearDto
{
    [Range(1, int.MaxValue)]
    public int IdMascota { get; set; }
}
