namespace BackendWebApi.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BackendWebApi.Data;
using BackendWebApi.Models.Core;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ExpedientesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ExpedientesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Expediente>>> GetExpedientes()
    {
        return await _context.Expedientes
            .Where(e => e.Habilitado)
            .OrderByDescending(e => e.FechaCreado)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Expediente>> GetExpediente(int id)
    {
        var expediente = await _context.Expedientes
            // .Include(e => e.Lotes)
            .FirstOrDefaultAsync(e => e.Id == id && e.Habilitado);

        if (expediente == null)
            return NotFound();

        return expediente;
    }

    [HttpPost]
    public async Task<ActionResult<Expediente>> CreateExpediente(Expediente expediente)
    {
        var usuario = User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";
        expediente.CreadoPor = usuario;
        expediente.FechaCreado = DateTime.Now;

        _context.Expedientes.Add(expediente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetExpediente), new { id = expediente.Id }, expediente);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpediente(int id, Expediente expediente)
    {
        if (id != expediente.Id)
            return BadRequest();

        var usuario = User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";
        expediente.ActualizadoPor = usuario;
        expediente.FechaActualizado = DateTime.Now;

        _context.Entry(expediente).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Expedientes.AnyAsync(e => e.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpediente(int id)
    {
        var expediente = await _context.Expedientes.FindAsync(id);
        if (expediente == null)
            return NotFound();

        var usuario = User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";
        expediente.Habilitado = false;
        expediente.ActualizadoPor = usuario;
        expediente.FechaActualizado = DateTime.Now;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}