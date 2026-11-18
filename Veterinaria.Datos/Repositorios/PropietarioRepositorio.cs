using Microsoft.EntityFrameworkCore;
using Veterinaria.Datos.Context;
using Veterinaria.Datos.Repositorios.Interfaces;
using Veterinaria.Entidades.Entidades;

namespace Veterinaria.Datos.Repositorios;

public class PropietarioRepositorio(VeterinariaContext context) : IPropietarioRepositorio
{
    public Task<List<Propietario>> ListarAsync() => context.Propietarios.AsNoTracking().OrderBy(p => p.Apellidos).ThenBy(p => p.Nombres).ToListAsync();
    public Task<Propietario?> ObtenerAsync(int id) => context.Propietarios.FirstOrDefaultAsync(p => p.IdPropietario == id);
    public Task<bool> ExisteDniAsync(string dni, int? idExcluir = null) => context.Propietarios.AnyAsync(p => p.Dni == dni && (idExcluir == null || p.IdPropietario != idExcluir));
    public Task<bool> TieneMascotasAsync(int id) => context.Mascotas.AnyAsync(m => m.IdPropietario == id);
    public Task AgregarAsync(Propietario propietario) => context.Propietarios.AddAsync(propietario).AsTask();
    public void Eliminar(Propietario propietario) => context.Propietarios.Remove(propietario);
    public Task GuardarAsync() => context.SaveChangesAsync();
}
