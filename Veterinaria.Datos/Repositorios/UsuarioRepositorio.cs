using Microsoft.EntityFrameworkCore;
using Veterinaria.Datos.Context;
using Veterinaria.Datos.Repositorios.Interfaces;
using Veterinaria.Entidades.Entidades;

namespace Veterinaria.Datos.Repositorios;

public class UsuarioRepositorio(VeterinariaContext context) : IUsuarioRepositorio
{
    public Task<Usuario?> ObtenerPorCorreoAsync(string correo) =>
        context.Usuarios.FirstOrDefaultAsync(usuario => usuario.Correo == correo);

    public Task AgregarAsync(Usuario usuario) => context.Usuarios.AddAsync(usuario).AsTask();

    public Task GuardarAsync() => context.SaveChangesAsync();
}
