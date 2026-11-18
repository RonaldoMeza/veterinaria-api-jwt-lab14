using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entidades.DTOs;

public class PropietarioCrearDto
{
    [Required, StringLength(80)]
    public string Nombres { get; set; } = null!;

    [Required, StringLength(80)]
    public string Apellidos { get; set; } = null!;

    [Required, StringLength(8), RegularExpression(@"^\d{8}$")]
    public string Dni { get; set; } = null!;

    [Required, StringLength(20)]
    public string Telefono { get; set; } = null!;

    [Required, StringLength(120), EmailAddress]
    public string Correo { get; set; } = null!;

    [Required, StringLength(200)]
    public string Direccion { get; set; } = null!;
}
