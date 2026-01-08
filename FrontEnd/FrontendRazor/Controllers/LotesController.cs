using Microsoft.AspNetCore.Mvc;

namespace FrontendRazor.Controllers;

using FrontendRazor.ViewModels;
using FrontEndRazor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FrontendRazor.DTOs;
using System.Text.Json;

[Authorize]
public class LotesController : Controller
{
    private readonly ApiService _apiService;

    public LotesController(ApiService apiService)
    {
        _apiService = apiService;
    }

    private bool TienePermiso(string accion)
    {
        var permisosJson = HttpContext.Session.GetString("Permisos");
        if (string.IsNullOrEmpty(permisosJson))
            return false;

        var permisos = JsonSerializer.Deserialize<List<PermisoDto>>(permisosJson);
        var permiso = permisos?.FirstOrDefault(p => p.Ventana.Equals("Lotes", StringComparison.OrdinalIgnoreCase));

        if (permiso == null) return false;

        return accion.ToLower() switch
        {
            "ver" => permiso.PuedeVer,
            "crear" => permiso.PuedeCrear,
            "editar" => permiso.PuedeEditar,
            "eliminar" => permiso.PuedeEliminar,
            _ => false
        };
    }

    public async Task<IActionResult> Index()
    {
        if (!TienePermiso("ver"))
        {
            return RedirectToAction("AccessDenied", "Auth");
        }

        ViewBag.PuedeCrear = TienePermiso("crear");
        ViewBag.PuedeEditar = TienePermiso("editar");
        ViewBag.PuedeEliminar = TienePermiso("eliminar");

        var lotes = await _apiService.GetAsync<List<LoteViewModel>>("lotes");
        return View(lotes ?? new List<LoteViewModel>());
    }

    public async Task<IActionResult> Details(int id)
    {
        if (!TienePermiso("ver"))
        {
            return RedirectToAction("AccessDenied", "Auth");
        }

        var lote = await _apiService.GetAsync<LoteViewModel>($"lotes/{id}");
        if (lote == null)
        {
            return NotFound();
        }

        return View(lote);
    }

    public async Task<IActionResult> Create()
    {
        if (!TienePermiso("crear"))
        {
            return RedirectToAction("AccessDenied", "Auth");
        }

        // Cargar expedientes para el dropdown
        var expedientes = await _apiService.GetAsync<List<ExpedienteViewModel>>("expedientes");
        ViewBag.Expedientes = expedientes ?? new List<ExpedienteViewModel>();

        return View(new LoteViewModel { Estado = "Activo" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LoteViewModel model)
    {
        if (!TienePermiso("crear"))
        {
            return RedirectToAction("AccessDenied", "Auth");
        }

        if (!ModelState.IsValid)
        {
            var expedientes = await _apiService.GetAsync<List<ExpedienteViewModel>>("expedientes");
            ViewBag.Expedientes = expedientes ?? new List<ExpedienteViewModel>();
            return View(model);
        }

        var success = await _apiService.PostAsync("lotes", model);

        if (success)
        {
            TempData["SuccessMessage"] = "Lote creado exitosamente";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, "Error al crear el lote");
        var expedientesReload = await _apiService.GetAsync<List<ExpedienteViewModel>>("expedientes");
        ViewBag.Expedientes = expedientesReload ?? new List<ExpedienteViewModel>();
        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        if (!TienePermiso("editar"))
        {
            return RedirectToAction("AccessDenied", "Auth");
        }

        var lote = await _apiService.GetAsync<LoteViewModel>($"lotes/{id}");
        if (lote == null)
        {
            return NotFound();
        }

        var expedientes = await _apiService.GetAsync<List<ExpedienteViewModel>>("expedientes");
        ViewBag.Expedientes = expedientes ?? new List<ExpedienteViewModel>();

        return View(lote);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, LoteViewModel model)
    {
        if (!TienePermiso("editar"))
        {
            return RedirectToAction("AccessDenied", "Auth");
        }

        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            var expedientes = await _apiService.GetAsync<List<ExpedienteViewModel>>("expedientes");
            ViewBag.Expedientes = expedientes ?? new List<ExpedienteViewModel>();
            return View(model);
        }

        var success = await _apiService.PutAsync($"lotes/{id}", model);

        if (success)
        {
            TempData["SuccessMessage"] = "Lote actualizado exitosamente";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, "Error al actualizar el lote");
        var expedientesReload = await _apiService.GetAsync<List<ExpedienteViewModel>>("expedientes");
        ViewBag.Expedientes = expedientesReload ?? new List<ExpedienteViewModel>();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (!TienePermiso("eliminar"))
        {
            return Json(new { success = false, message = "No tienes permisos para eliminar" });
        }

        var success = await _apiService.DeleteAsync($"lotes/{id}");

        if (success)
        {
            return Json(new { success = true, message = "Lote eliminado exitosamente" });
        }

        return Json(new { success = false, message = "Error al eliminar el lote" });
    }
}