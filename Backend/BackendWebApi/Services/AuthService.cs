namespace BackendWebApi.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackendWebApi.Data;
using BackendWebApi.DTOs;
using BackendWebApi.Models.Identity;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.UsuariosRoles)
                .ThenInclude(ur => ur.Rol)
            .FirstOrDefaultAsync(u => u.NombreUsuario == request.NombreUsuario && u.Activo);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
            return null;

        // Actualizar último acceso
        usuario.UltimoAcceso = DateTime.Now;
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(usuario);
        var roles = usuario.UsuariosRoles.Select(ur => ur.Rol.NombreRol).ToList();

        return new LoginResponseDto
        {
            Token = token,
            Usuario = new UsuarioDto
            {
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                Email = usuario.Email,
                NombreCompleto = usuario.NombreCompleto,
                Roles = roles
            }
        };
    }

    public async Task<Usuario> GetUserByIdAsync(int userId)
    {
        return await _context.Usuarios
            .Include(u => u.UsuariosRoles)
                .ThenInclude(ur => ur.Rol)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    private string GenerateJwtToken(Usuario usuario)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.NombreUsuario),
            new Claim(ClaimTypes.Email, usuario.Email)
        };

        foreach (var ur in usuario.UsuariosRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, ur.Rol.NombreRol));
        }

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["ExpirationMinutes"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}