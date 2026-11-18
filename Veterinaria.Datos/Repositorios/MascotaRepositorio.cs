using Microsoft.EntityFrameworkCore;
using Veterinaria.Datos.Context;
using Veterinaria.Datos.Repositorios.Interfaces;
using Veterinaria.Entidades.Entidades;

namespace Veterinaria.Datos.Repositorios;

public class MascotaRepositorio(VeterinariaContext context) : IMascotaRepositorio
{
    public async Task<List<Mascota>> ListarAsync(string? especie, string? nombre)
    {
        IQueryable<Mascota> consulta = context.Mascotas.Include(m => m.Propietario).AsNoTracking();
        if (!string.IsNullOrWhiteSpace(especie)) consulta = consulta.Where(m => m.Especie.Contains(especie));
        if (!string.IsNullOrWhiteSpace(nombre)) consulta = consulta.Where(m => m.Nombre.Contains(nombre));
        return await consulta.OrderBy(m => m.Nombre).ToListAsync();
    }

    public Task<Mascota?> ObtenerAsync(int id) => context.Mascotas.Include(m => m.Propietario).FirstOrDefaultAsync(m => m.IdMascota == id);
    public Task<List<Mascota>> PorPropietarioAsync(int id) => context.Mascotas.Include(m => m.Propietario).AsNoTracking().Where(m => m.IdPropietario == id).OrderBy(m => m.Nombre).ToListAsync();
    public Task<bool> PropietarioExisteAsync(int id) => context.Propietarios.AnyAsync(p => p.IdPropietario == id);
    public Task AgregarAsync(Mascota mascota) => context.Mascotas.AddAsync(mascota).AsTask();
    public void Eliminar(Mascota mascota) => context.Mascotas.Remove(mascota);
    public Task GuardarAsync() => context.SaveChangesAsync();
}
