using Microsoft.AspNetCore.Mvc;

namespace FrontendRazor.Controllers;

using FrontendRazor.ViewModels;
using FrontEndRazor.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FrontendRazor.DTOs;
using System.Security.Claims;
using System.Text.Json;

public class AuthController : Controller
{
    private readonly ApiService _apiService;

    public AuthController(ApiService apiService)
    {
        _apiService = apiService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        // Si ya está autenticado, redirigir al home
        if (User.Identity?.IsAuthenticated ?? false)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var (success, error) = await _apiService.LoginAsync(model.NombreUsuario, model.Password);

        if (!success)
        {
            ModelState.AddModelError(string.Empty, error ?? "Error al iniciar sesión");
            return View(model);
        }

        // Obtener datos del usuario desde la sesión
        var usuarioJson = HttpContext.Session.GetString("Usuario");
        if (string.IsNullOrEmpty(usuarioJson))
        {
            ModelState.AddModelError(string.Empty, "Error al procesar la autenticación");
            return View(model);
        }

        var usuario = JsonSerializer.Deserialize<UsuarioDto>(usuarioJson);
        if (usuario == null)
        {
            ModelState.AddModelError(string.Empty, "Error al procesar los datos del usuario");
            return View(model);
        }

        // Crear claims para la cookie de autenticación
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.NombreUsuario),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim("NombreCompleto", usuario.NombreCompleto)
        };

        // Agregar roles como claims
        foreach (var rol in usuario.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, rol));
        }

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        // Cargar permisos del usuario
        var permisos = await _apiService.GetPermisosAsync();
        if (permisos != null)
        {
            HttpContext.Session.SetString("Permisos", JsonSerializer.Serialize(permisos));
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
