using AdresApp.Domain.Entities;

namespace AdresApp.Domain.Interfaces;

public interface ICargueArchivoRepository
{
    Task<CargueArchivo?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CargueArchivo>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<int> AgregarAsync(CargueArchivo cargueArchivo, CancellationToken cancellationToken = default);
    Task ActualizarAsync(CargueArchivo cargueArchivo, CancellationToken cancellationToken = default);
}
