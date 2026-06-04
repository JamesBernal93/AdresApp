namespace AdresApp.Domain.Entities;

public class CargueArchivo
{
    public int Id { get; private set; }
    public int IdArchivo { get; private set; }
    public int IdPrestador { get; private set; }
    public string NombreArchivo { get; private set; } = string.Empty;
    public string Estado { get; private set; } = "PENDIENTE";
    public DateTime FechaCreacion { get; private set; }
    public DateTime? FechaActualizacion { get; private set; }
    public string? RespuestaCargue { get; private set; }
    public string? RespuestaEstado { get; private set; }
    public string? MensajeError { get; private set; }

    private CargueArchivo() { }

    public static CargueArchivo Crear(int idArchivo, int idPrestador, string nombreArchivo)
    {
        if (idArchivo <= 0) throw new ArgumentException("IdArchivo debe ser mayor a cero.", nameof(idArchivo));
        if (idPrestador <= 0) throw new ArgumentException("IdPrestador debe ser mayor a cero.", nameof(idPrestador));
        if (string.IsNullOrWhiteSpace(nombreArchivo)) throw new ArgumentException("El nombre del archivo no puede estar vacío.", nameof(nombreArchivo));

        return new CargueArchivo
        {
            IdArchivo = idArchivo,
            IdPrestador = idPrestador,
            NombreArchivo = nombreArchivo,
            Estado = "PENDIENTE",
            FechaCreacion = DateTime.UtcNow
        };
    }

    public void RegistrarRespuestaCargue(string respuesta)
    {
        RespuestaCargue = respuesta ?? throw new ArgumentNullException(nameof(respuesta));
        Estado = "CARGADO";
        FechaActualizacion = DateTime.UtcNow;
    }

    public void RegistrarRespuestaEstado(string respuesta)
    {
        RespuestaEstado = respuesta ?? throw new ArgumentNullException(nameof(respuesta));
        FechaActualizacion = DateTime.UtcNow;
    }

    public void MarcarError(string mensaje)
    {
        MensajeError = mensaje;
        Estado = "ERROR";
        FechaActualizacion = DateTime.UtcNow;
    }
}
