namespace AdresApp.Application.DTOs;

public record CargueArchivoRequestDto(int IdArchivo, int IdPrestador);

public record CargueArchivoResponseDto(
    int Id, int IdArchivo, int IdPrestador, string NombreArchivo,
    string Estado, DateTime FechaCreacion, DateTime? FechaActualizacion,
    string? RespuestaCargue, string? RespuestaEstado, string? MensajeError);

public record ConsultaEstadoResponseDto(int Id, string Estado, string? RespuestaEstado, DateTime? FechaActualizacion);

public record LoginRequestDto(string Usuario, string Password);

public record LoginResponseDto(string Token, DateTime Expiracion);

public record ApiResponseDto<T>(bool Exitoso, string? Mensaje, T? Data)
{
    public static ApiResponseDto<T> Ok(T data, string? mensaje = null) => new(true, mensaje, data);
    public static ApiResponseDto<T> Error(string mensaje) => new(false, mensaje, default);
}
