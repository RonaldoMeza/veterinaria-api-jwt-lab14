using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entidades.DTOs;

public class MascotaCrearDto
{
    [Required, StringLength(80)]
    public string Nombre { get; set; } = null!;

    [Required, StringLength(50)]
    public string Especie { get; set; } = null!;

    [Required, StringLength(80)]
    public string Raza { get; set; } = null!;

    [Required, StringLength(1), RegularExpression("^[MH]$")]
    public string Sexo { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    [Range(0.01, 500)]
    public decimal Peso { get; set; }

    [Range(1, int.MaxValue)]
    public int IdPropietario { get; set; }
}
