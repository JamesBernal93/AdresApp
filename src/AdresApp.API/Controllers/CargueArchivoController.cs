using AdresApp.Application.DTOs;
using AdresApp.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdresApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
[Produces("application/json")]
public class CargueArchivoController : ControllerBase
{
    private readonly ICargueArchivoService _service;
    private readonly IValidator<CargueArchivoRequestDto> _validator;

    public CargueArchivoController(ICargueArchivoService service, IValidator<CargueArchivoRequestDto> validator)
    {
        _service = service;
        _validator = validator;
    }

    /// <summary>Obtiene todos los registros de cargue.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<CargueArchivoResponseDto>>), 200)]
    public async Task<IActionResult> ObtenerTodos(CancellationToken cancellationToken)
    {
        var resultado = await _service.ObtenerTodosAsync(cancellationToken);
        return Ok(ApiResponseDto<IEnumerable<CargueArchivoResponseDto>>.Ok(resultado));
    }

    /// <summary>Carga un archivo al servicio ADRES.</summary>
    [HttpPost("cargar")]
    [ProducesResponseType(typeof(ApiResponseDto<CargueArchivoResponseDto>), 201)]
    [ProducesResponseType(typeof(ApiResponseDto<object>), 400)]
    [ProducesResponseType(typeof(ApiResponseDto<object>), 502)]
    public async Task<IActionResult> CargarArchivo(
        [FromForm] int idArchivo,
        [FromForm] int idPrestador,
        IFormFile archivo,
        CancellationToken cancellationToken)
    {
        var request = new CargueArchivoRequestDto(idArchivo, idPrestador);
        var validacion = await _validator.ValidateAsync(request, cancellationToken);
        if (!validacion.IsValid)
            return BadRequest(ApiResponseDto<object>.Error(string.Join("; ", validacion.Errors.Select(e => e.ErrorMessage))));

        if (archivo is null || archivo.Length == 0)
            return BadRequest(ApiResponseDto<object>.Error("Debe adjuntar un archivo valido."));

        await using var stream = archivo.OpenReadStream();
        var resultado = await _service.CargarArchivoAsync(request, stream, archivo.FileName, cancellationToken);
        return CreatedAtAction(nameof(ConsultarEstado), new { id = resultado.Id },
            ApiResponseDto<CargueArchivoResponseDto>.Ok(resultado, "Archivo cargado exitosamente."));
    }

    /// <summary>Consulta el estado de un cargue en ADRES.</summary>
    [HttpGet("{id:int}/estado")]
    [ProducesResponseType(typeof(ApiResponseDto<ConsultaEstadoResponseDto>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<object>), 404)]
    public async Task<IActionResult> ConsultarEstado(int id, CancellationToken cancellationToken)
    {
        var resultado = await _service.ConsultarEstadoAsync(id, cancellationToken);
        return Ok(ApiResponseDto<ConsultaEstadoResponseDto>.Ok(resultado));
    }
}
