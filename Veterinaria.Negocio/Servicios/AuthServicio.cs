using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Veterinaria.Datos.Repositorios.Interfaces;
using Veterinaria.Entidades.DTOs;
using Veterinaria.Entidades.Entidades;
using Veterinaria.Negocio.Configuracion;
using Veterinaria.Negocio.Excepciones;
using Veterinaria.Negocio.Servicios.Interfaces;

namespace Veterinaria.Negocio.Servicios;

public class AuthServicio(IUsuarioRepositorio usuarios, JwtOpciones jwt) : IAuthServicio
{
    private const string RolRegistro = "Recepcionista";

    public async Task RegistrarAsync(RegistroDto dto)
    {
        var correoNormalizado = dto.Correo.Trim().ToLowerInvariant();
        if (await usuarios.ObtenerPorCorreoAsync(correoNormalizado) is not null)
            throw new ReglaNegocioException("Ya existe un usuario con ese correo.");

        var usuario = new Usuario
        {
            Correo = correoNormalizado,
            ClaveHash = BCrypt.Net.BCrypt.HashPassword(dto.Clave),
            Rol = RolRegistro
        };

        await usuarios.AgregarAsync(usuario);
        await usuarios.GuardarAsync();
    }

    public async Task<TokenDto?> LoginAsync(LoginDto dto)
    {
        var correoNormalizado = dto.Correo.Trim().ToLowerInvariant();
        var usuario = await usuarios.ObtenerPorCorreoAsync(correoNormalizado);

        if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Clave, usuario.ClaveHash))
            return null;

        return GenerarToken(usuario);
    }

    private TokenDto GenerarToken(Usuario usuario)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.IdUsuario.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Correo),
            new Claim(ClaimTypes.Role, usuario.Rol)
        };

        var expiracion = DateTime.UtcNow.AddMinutes(jwt.Minutos);
        var credenciales = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Clave)),
            SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: jwt.Emisor,
            audience: jwt.Audiencia,
            claims: claims,
            expires: expiracion,
            signingCredentials: credenciales);

        return new TokenDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expira = expiracion
        };
    }
}
