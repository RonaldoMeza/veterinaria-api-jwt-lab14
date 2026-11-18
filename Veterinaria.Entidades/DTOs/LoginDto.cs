using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entidades.DTOs;

public class LoginDto
{
    [Required, StringLength(120), EmailAddress]
    public string Correo { get; set; } = null!;

    [Required, StringLength(72, MinimumLength = 8)]
    public string Clave { get; set; } = null!;
}
