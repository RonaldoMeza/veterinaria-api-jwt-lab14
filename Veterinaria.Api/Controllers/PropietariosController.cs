using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Entidades.DTOs;
using Veterinaria.Negocio.Excepciones;
using Veterinaria.Negocio.Servicios.Interfaces;

namespace Veterinaria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PropietariosController(IPropietarioServicio servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PropietarioDto>>> Get() => Ok(await servicio.ListarAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PropietarioDto>> Get(int id)
    {
        var propietario = await servicio.ObtenerAsync(id);
        return propietario is null ? NotFound() : Ok(propietario);
    }

    [HttpGet("{id:int}/mascotas")]
    public async Task<ActionResult<IEnumerable<MascotaDto>>> Mascotas(int id)
    {
        var mascotas = await servicio.MascotasAsync(id);
        return mascotas is null ? NotFound() : Ok(mascotas);
    }

    [HttpPost]
    [Authorize(Roles = "Recepcionista")]
    public async Task<ActionResult<PropietarioDto>> Post(PropietarioCrearDto dto)
    {
        try
        {
            var propietario = await servicio.CrearAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = propietario.IdPropietario }, propietario);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new ProblemDetails { Title = "Regla de negocio", Detail = ex.Message, Status = 409 });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Recepcionista")]
    public async Task<IActionResult> Put(int id, PropietarioActualizarDto dto)
    {
        try
        {
            return await servicio.ActualizarAsync(id, dto) ? NoContent() : NotFound();
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (ReglaNegocioException ex) { return Conflict(new ProblemDetails { Title = "Regla de negocio", Detail = ex.Message, Status = 409 }); }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Recepcionista")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            return await servicio.EliminarAsync(id) ? NoContent() : NotFound();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new ProblemDetails { Title = "Regla de negocio", Detail = ex.Message, Status = 409 });
        }
    }
}
