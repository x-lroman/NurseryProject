namespace BackendWebApi.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BackendWebApi.DTOs;
using BackendWebApi.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IPermisosService _permisosService;

    public AuthController(IAuthService authService, IPermisosService permisosService)
    {
        _authService = authService;
        _permisosService = permisosService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var response = await _authService.LoginAsync(request);
        if (response == null)
            return Unauthorized(new { message = "Credenciales inválidas" });

        return Ok(response);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var usuario = await _authService.GetUserByIdAsync(userId);

        if (usuario == null)
            return NotFound();

        return Ok(new UsuarioDto
        {
            Id = usuario.Id,
            NombreUsuario = usuario.NombreUsuario,
            Email = usuario.Email,
            NombreCompleto = usuario.NombreCompleto,
            Roles = usuario.UsuariosRoles.Select(ur => ur.Rol.NombreRol).ToList()
        });
    }

    [Authorize]
    [HttpGet("permisos")]
    public async Task<IActionResult> GetPermisos()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var permisos = await _permisosService.GetPermisosByUsuarioAsync(userId);
        return Ok(permisos);
    }
}