using AdresApp.Application.DTOs;
using AdresApp.Application.Interfaces;
using AdresApp.Domain.Entities;
using AdresApp.Domain.Exceptions;
using AdresApp.Domain.Interfaces;

namespace AdresApp.Application.Services;

public class CargueArchivoService : ICargueArchivoService
{
    private readonly ICargueArchivoRepository _repository;
    private readonly IAdresExternalService _adresService;

    public CargueArchivoService(ICargueArchivoRepository repository, IAdresExternalService adresService)
    {
        _repository = repository;
        _adresService = adresService;
    }

    public async Task<CargueArchivoResponseDto> CargarArchivoAsync(
        CargueArchivoRequestDto request, Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        var entidad = CargueArchivo.Crear(request.IdArchivo, request.IdPrestador, fileName);
        await _repository.AgregarAsync(entidad, cancellationToken);

        try
        {
            var respuesta = await _adresService.CargarArchivoAsync(request.IdArchivo, request.IdPrestador, fileStream, fileName, cancellationToken);
            entidad.RegistrarRespuestaCargue(respuesta);
            await _repository.ActualizarAsync(entidad, cancellationToken);
        }
        catch (Exception ex)
        {
            entidad.MarcarError(ex.Message);
            await _repository.ActualizarAsync(entidad, cancellationToken);
            throw new ExternalServiceException("Error al cargar el archivo en ADRES.", ex);
        }

        return MapToDto(entidad);
    }

    public async Task<ConsultaEstadoResponseDto> ConsultarEstadoAsync(int id, CancellationToken cancellationToken = default)
    {
        var entidad = await _repository.ObtenerPorIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(CargueArchivo), id);

        try
        {
            var respuesta = await _adresService.ConsultarEstadoCargueAsync(entidad.IdArchivo, cancellationToken);
            entidad.RegistrarRespuestaEstado(respuesta);
            await _repository.ActualizarAsync(entidad, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ExternalServiceException("Error al consultar el estado del cargue.", ex);
        }

        return new ConsultaEstadoResponseDto(entidad.Id, entidad.Estado, entidad.RespuestaEstado, entidad.FechaActualizacion);
    }

    public async Task<IEnumerable<CargueArchivoResponseDto>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var lista = await _repository.ObtenerTodosAsync(cancellationToken);
        return lista.Select(MapToDto);
    }

    private static CargueArchivoResponseDto MapToDto(CargueArchivo e) =>
        new(e.Id, e.IdArchivo, e.IdPrestador, e.NombreArchivo, e.Estado,
            e.FechaCreacion, e.FechaActualizacion, e.RespuestaCargue, e.RespuestaEstado, e.MensajeError);
}
