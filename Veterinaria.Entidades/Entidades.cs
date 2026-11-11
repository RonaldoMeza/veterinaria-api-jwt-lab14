namespace Veterinaria.Entidades;

public partial class Propietario
{
    public int IdPropietario { get; set; }
    public string Nombres { get; set; } = null!;
    public string Apellidos { get; set; } = null!;
    public string Dni { get; set; } = null!;
    public string Telefono { get; set; } = null!;
    public string Correo { get; set; } = null!;
    public string Direccion { get; set; } = null!;
    public virtual ICollection<Mascota> Mascotas { get; set; } = new List<Mascota>();
}

public partial class Mascota
{
    public int IdMascota { get; set; }
    public string Nombre { get; set; } = null!;
    public string Especie { get; set; } = null!;
    public string Raza { get; set; } = null!;
    public string Sexo { get; set; } = null!;
    public DateOnly FechaNacimiento { get; set; }
    public decimal Peso { get; set; }
    public int IdPropietario { get; set; }
    public virtual Propietario? Propietario { get; set; }
}
