using Microsoft.AspNetCore.Mvc;

namespace FrontendRazor.Controllers;

using FrontEndRazor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FrontendRazor.DTOs;
using System.Text.Json;

[Authorize]
public class HomeController : Controller
{
    private readonly ApiService _apiService;

    public HomeController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public IActionResult Index()
    {
        // Obtener permisos de la sesión para mostrar en la vista
        var permisosJson = HttpContext.Session.GetString("Permisos");
        if (!string.IsNullOrEmpty(permisosJson))
        {
            var permisos = JsonSerializer.Deserialize<List<PermisoDto>>(permisosJson);
            ViewBag.Permisos = permisos;
        }

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}