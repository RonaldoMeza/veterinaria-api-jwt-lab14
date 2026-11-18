namespace Veterinaria.Entidades.DTOs;

public class MascotaDto
{
    public int IdMascota { get; set; }
    public string Nombre { get; set; } = null!;
    public string Especie { get; set; } = null!;
    public string Raza { get; set; } = null!;
    public string Sexo { get; set; } = null!;
    public DateOnly FechaNacimiento { get; set; }
    public decimal Peso { get; set; }
    public int IdPropietario { get; set; }
    public string NombrePropietario { get; set; } = null!;
}
