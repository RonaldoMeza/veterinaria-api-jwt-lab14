namespace Veterinaria.Entidades.DTOs;

public class PropietarioDto
{
    public int IdPropietario { get; set; }
    public string Nombres { get; set; } = null!;
    public string Apellidos { get; set; } = null!;
    public string Dni { get; set; } = null!;
    public string Telefono { get; set; } = null!;
    public string Correo { get; set; } = null!;
    public string Direccion { get; set; } = null!;
}
