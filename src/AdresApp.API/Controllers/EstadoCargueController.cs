using Microsoft.AspNetCore.Mvc;
using AdresApp.Application.Services;
using AdresApp.Application.DTOs;

namespace AdresApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoCargueController : ControllerBase
    {
        private readonly CargueEstadoService _service;

        public EstadoCargueController(CargueEstadoService service)
        {
            _service = service;
        }

        [HttpGet("GetEstadoCargue")]
        public async Task<IActionResult> GetEstadoCargue([FromQuery] int idCargueArchivo, CancellationToken ct)
        {
            var estado = await _service.ObtenerEstadoAsync(idCargueArchivo, ct);

            if (estado.ReporteEjecucion.Porcentaje < 100)
                return Ok(new { mensaje = "Archivo no ha sido procesado", porcentaje = estado.ReporteEjecucion.Porcentaje });

            return Ok(estado);
        }
    }
}
