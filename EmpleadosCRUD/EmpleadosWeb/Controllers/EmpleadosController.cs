
using Microsoft.AspNetCore.Mvc;
using EmpleadosWeb.Models;
using EmpleadosWeb.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmpleadosWeb.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly EmpleadoData _empleadoData;

        public EmpleadosController(EmpleadoData empleadoData)
        {
            _empleadoData = empleadoData;
        }

        // Vista principal
        public IActionResult Index()
        {
            return View();
        }

        // Renderiza el formulario de creación
        public IActionResult CreatePartial()
        {
            return PartialView("Partials/_FormCreate", new Empleado());
        }

        // Renderiza el formulario de edición
        public IActionResult EditPartial(int id)
        {
            var empleado = _empleadoData.ObtenerAsync(id);
            if (empleado == null)
                return NotFound();
            return PartialView("Partials/_FormEdit", empleado);
        }

        // Renderiza la lista de empleados
        public IActionResult ListarAjax()
        {
            var empleados = _empleadoData.ListarAsync();
            return PartialView("Partials/_ListaEmpleados", empleados);
        }

        // Guardar nuevo empleado
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Empleado model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos inválidos" });

            var result = await _empleadoData.GuardarAsync(model);
            return Json(new { success = result });
        }

        // Editar empleado existente
        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] Empleado model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos inválidos" });

            var result = await _empleadoData.EditarAsync(model);
            return Json(new { success = result });
        }

        // Eliminar empleado
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _empleadoData.EliminarAsync(id);
            return Json(new { success = result });
        }
    }
}
