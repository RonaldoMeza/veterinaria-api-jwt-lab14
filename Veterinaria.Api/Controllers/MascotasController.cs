using Microsoft.AspNetCore.Mvc;
using Veterinaria.Entidades.DTOs;
using Veterinaria.Negocio.Servicios.Interfaces;

namespace Veterinaria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MascotasController(IMascotaServicio servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MascotaDto>>> Get(string? especie, string? nombre) => Ok(await servicio.ListarAsync(especie, nombre));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MascotaDto>> Get(int id)
    {
        var mascota = await servicio.ObtenerAsync(id);
        return mascota is null ? NotFound() : Ok(mascota);
    }

    [HttpPost]
    public async Task<ActionResult<MascotaDto>> Post(MascotaCrearDto dto)
    {
        try
        {
            var mascota = await servicio.CrearAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = mascota.IdMascota }, mascota);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, MascotaActualizarDto dto)
    {
        try
        {
            return await servicio.ActualizarAsync(id, dto) ? NoContent() : NotFound();
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => await servicio.EliminarAsync(id) ? NoContent() : NotFound();
}
