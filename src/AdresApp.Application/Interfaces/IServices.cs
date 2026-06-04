using AdresApp.Application.DTOs;

namespace AdresApp.Application.Interfaces;

public interface ICargueArchivoService
{
    Task<CargueArchivoResponseDto> CargarArchivoAsync(CargueArchivoRequestDto request, Stream fileStream, string fileName, CancellationToken cancellationToken = default);
    Task<ConsultaEstadoResponseDto> ConsultarEstadoAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CargueArchivoResponseDto>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
}

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
}
