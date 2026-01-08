using Microsoft.AspNetCore.Mvc;

namespace FrontendRazor.Controllers;

using FrontendRazor.ViewModels;
using FrontEndRazor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FrontendRazor.DTOs;
using System.Text.Json;

[Authorize]
public class ExpedientesController : Controller
{
    private readonly ApiService _apiService;

    public ExpedientesController(ApiService apiService)
    {
        _apiService = apiService;
    }

    private bool TienePermiso(string accion)
    {
        var permisosJson = HttpContext.Session.GetString("Permisos");
        if (string.IsNullOrEmpty(permisosJson))
            return false;

        var permisos = JsonSerializer.Deserialize<List<PermisoDto>>(permisosJson);
        var permiso = permisos?.FirstOrDefault(p => p.Ventana.Equals("Expedientes", StringComparison.OrdinalIgnoreCase));

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

    // GET: Expedientes
    public async Task<IActionResult> Index()
    {
        if (!TienePermiso("ver"))
        {
            return RedirectToAction("AccessDenied", "Auth");
        }

        ViewBag.PuedeCrear = TienePermiso("crear");
        ViewBag.PuedeEditar = TienePermiso("editar");
        ViewBag.PuedeEliminar = TienePermiso("eliminar");

        var expedientes = await _apiService.GetAsync<List<ExpedienteViewModel>>("expedientes");
        return View(expedientes ?? new List<ExpedienteViewModel>());
    }

    // GET: Expedientes/Details/5
    public async Task<IActionResult> Details(int id)
    {
        if (!TienePermiso("ver"))
        {
            return RedirectToAction("AccessDenied", "Auth");
        }

        var expediente = await _apiService.GetAsync<ExpedienteViewModel>($"expedientes/{id}");
        if (expediente == null)
        {
            return NotFound();
        }

        return View(expediente);
    }

    // GET: Expedientes/Create
    public IActionResult Create()
    {
        if (!TienePermiso("crear"))
        {
            return RedirectToAction("AccessDenied", "Auth");
        }

        var viewModel = new ExpedienteViewModel
        {
            FechaIngreso = DateTime.Now,
            Estado = "Activo"
        };

        return View(viewModel);
    }

    // POST: Expedientes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ExpedienteViewModel model)
    {
        if (!TienePermiso("crear"))
        {
            return RedirectToAction("AccessDenied", "Auth");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }
        model.CreadoPor = "yo";
        model.FechaCreado = DateTime.Now;

        var success = await _apiService.PostAsync("expedientes", model);

        if (success)
        {
            TempData["SuccessMessage"] = "Expediente creado exitosamente";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, "Error al crear el expediente");
        return View(model);
    }

    // GET: Expedientes/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        if (!TienePermiso("editar"))
        {
            return RedirectToAction("AccessDenied", "Auth");
        }

        var expediente = await _apiService.GetAsync<ExpedienteViewModel>($"expedientes/{id}");
        if (expediente == null)
        {
            return NotFound();
        }

        return View(expediente);
    }

    // POST: Expedientes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ExpedienteViewModel model)
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
            return View(model);
        }

        var success = await _apiService.PutAsync($"expedientes/{id}", model);

        if (success)
        {
            TempData["SuccessMessage"] = "Expediente actualizado exitosamente";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, "Error al actualizar el expediente");
        return View(model);
    }

    // POST: Expedientes/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (!TienePermiso("eliminar"))
        {
            return Json(new { success = false, message = "No tienes permisos para eliminar" });
        }

        var success = await _apiService.DeleteAsync($"expedientes/{id}");

        if (success)
        {
            return Json(new { success = true, message = "Expediente eliminado exitosamente" });
        }

        return Json(new { success = false, message = "Error al eliminar el expediente" });
    }
}