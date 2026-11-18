using Veterinaria.Entidades.Entidades;

namespace Veterinaria.Datos.Repositorios.Interfaces;

public interface IMascotaRepositorio
{
    Task<List<Mascota>> ListarAsync(string? especie, string? nombre);
    Task<Mascota?> ObtenerAsync(int id);
    Task<List<Mascota>> PorPropietarioAsync(int id);
    Task<bool> PropietarioExisteAsync(int id);
    Task AgregarAsync(Mascota mascota);
    void Eliminar(Mascota mascota);
    Task GuardarAsync();
}
