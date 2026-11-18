using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Entidades.DTOs;
using Veterinaria.Negocio.Excepciones;
using Veterinaria.Negocio.Servicios.Interfaces;

namespace Veterinaria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthServicio authServicio) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("registro")]
    public async Task<IActionResult> Registro(RegistroDto dto)
    {
        try
        {
            await authServicio.RegistrarAsync(dto);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new ProblemDetails
            {
                Title = "Regla de negocio",
                Detail = ex.Message,
                Status = StatusCodes.Status409Conflict
            });
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login(LoginDto dto)
    {
        var token = await authServicio.LoginAsync(dto);
        return token is null ? Unauthorized() : Ok(token);
    }

    [Authorize]
    [HttpGet("perfil")]
    public ActionResult<PerfilDto> Perfil()
    {
        var correo = User.FindFirstValue(ClaimTypes.Email)
            ?? User.FindFirstValue(JwtRegisteredClaimNames.Email);
        var rol = User.FindFirstValue(ClaimTypes.Role);

        return Ok(new PerfilDto
        {
            Correo = correo ?? string.Empty,
            Rol = rol ?? string.Empty
        });
    }
}
