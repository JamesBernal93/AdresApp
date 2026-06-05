using System.Threading;
using System.Threading.Tasks;

namespace AdresApp.Domain.Interfaces
{
    public interface IAdresExternalService
    {
        Task<string> CargarArchivoAsync(int idArchivo, int idPrestador, Stream fileStream, string fileName, CancellationToken cancellationToken = default);
        Task<string> ConsultarEstadoCargueAsync(int idCargueArchivo, CancellationToken cancellationToken = default);
        Task<string> GetReporteEjecucionAsync(int idCargueArchivo, CancellationToken ct = default);
        Task<string> GetReporteDetalleCargueArchivoGlosasAsync(int idCargueArchivo, CancellationToken ct = default);
    }
}
