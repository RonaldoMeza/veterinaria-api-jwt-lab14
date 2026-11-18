using Veterinaria.Entidades.Entidades;

namespace Veterinaria.Datos.Repositorios.Interfaces;

public interface IPropietarioRepositorio
{
    Task<List<Propietario>> ListarAsync();
    Task<Propietario?> ObtenerAsync(int id);
    Task<bool> ExisteDniAsync(string dni, int? idExcluir = null);
    Task<bool> TieneMascotasAsync(int id);
    Task AgregarAsync(Propietario propietario);
    void Eliminar(Propietario propietario);
    Task GuardarAsync();
}
