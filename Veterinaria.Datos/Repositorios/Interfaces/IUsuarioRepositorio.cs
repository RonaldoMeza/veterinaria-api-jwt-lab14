using Veterinaria.Entidades.Entidades;

namespace Veterinaria.Datos.Repositorios.Interfaces;

public interface IUsuarioRepositorio
{
    Task<Usuario?> ObtenerPorCorreoAsync(string correo);
    Task AgregarAsync(Usuario usuario);
    Task GuardarAsync();
}
