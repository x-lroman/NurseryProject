namespace BackendWebApi.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BackendWebApi.Data;
using BackendWebApi.Models.Identity;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UsuariosController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
    {
        return await _context.Usuarios
            .Where(e => e.Activo)
            .OrderByDescending(e => e.FechaCreacion)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        var Usuario = await _context.Usuarios
            .Include(e => e.UsuariosRoles)
            .FirstOrDefaultAsync(e => e.Id == id && e.Activo);

        if (Usuario == null)
            return NotFound();

        return Usuario;
    }

    [HttpPost]
    public async Task<ActionResult<Usuario>> CreateUsuario(Usuario Usuario)
    {
        var usuario = User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";
        Usuario.FechaCreacion = DateTime.Now;

        _context.Usuarios.Add(Usuario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUsuario), new { id = Usuario.Id }, Usuario);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUsuario(int id, Usuario Usuario)
    {
        if (id != Usuario.Id)
            return BadRequest();

        var usuario = User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";

        var usuarioDB = await _context.Usuarios
            .Include(e => e.UsuariosRoles)
            .FirstOrDefaultAsync(e => e.Id == id && e.Activo);

        usuarioDB.PasswordHash = BCrypt.Net.BCrypt.HashPassword(Usuario.PasswordHash, workFactor: 12);
        _context.Entry(usuarioDB).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Usuarios.AnyAsync(e => e.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        var Usuario = await _context.Usuarios.FindAsync(id);
        if (Usuario == null)
            return NotFound();

        var usuario = User.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";
        Usuario.Activo = false;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}