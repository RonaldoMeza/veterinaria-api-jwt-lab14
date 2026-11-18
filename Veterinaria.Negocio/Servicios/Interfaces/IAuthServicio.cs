using Veterinaria.Entidades.DTOs;

namespace Veterinaria.Negocio.Servicios.Interfaces;

public interface IAuthServicio
{
    Task RegistrarAsync(RegistroDto dto);
    Task<TokenDto?> LoginAsync(LoginDto dto);
}
