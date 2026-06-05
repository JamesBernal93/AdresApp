using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AdresApp.Domain.Exceptions;
using AdresApp.Domain.Interfaces;

namespace AdresApp.Infrastructure.External
{
    public class AdresExternalService : IAdresExternalService
    {
        private readonly HttpClient _client;
        private readonly ILogger<AdresExternalService> _logger;
        string _baseUrl;
        string _aa = "https://serviciostramites-pru.adres.gov.co:444";

        public AdresExternalService(HttpClient client, ILogger<AdresExternalService> logger, IConfiguration configuration)
        {
            _client = client;
            _logger = logger;
            _baseUrl = configuration["AdresService:BaseUrl"]
                ?? throw new InvalidOperationException("AdresService:BaseUrl no esta configurado.");
            _logger.LogInformation("AdresService:BaseUrl = {BaseUrl}", _baseUrl);
        }

        public async Task<string> CargarArchivoAsync(
            int idArchivo, int idPrestador, Stream fileStream, string fileName,
            CancellationToken cancellationToken = default)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(idArchivo.ToString()), "idArchivo");
            content.Add(new StringContent(idPrestador.ToString()), "idPrestador");

            var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            content.Add(streamContent, "fileStream", fileName);

            var response = await _client.PostAsync(
                _baseUrl+$"/api/CargueArchivo/GuardarArchivoAsync", content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new ExternalServiceException($"Error al cargar archivo. HTTP {(int)response.StatusCode}: {error}");
            }

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }

        public async Task<string> ConsultarEstadoCargueAsync(int idCargueArchivo, CancellationToken cancellationToken = default)
        {
            var response = await _client.PostAsync(
                $"{_aa}/api/CargueArchivo/GetRerpoteCargueArchivo?idCargueArchivo={idCargueArchivo}",
                new StringContent(string.Empty), cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new ExternalServiceException($"Error al consultar estado. HTTP {(int)response.StatusCode}: {error}");
            }

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }

        public async Task<string> GetReporteEjecucionAsync(int idCargueArchivo, CancellationToken ct = default)
        {
            string url = _aa + $"/api/CargueArchivo/GetReporteEjecucionAsync?idCargueArchivo={idCargueArchivo}";
            _logger.LogInformation("Requesting ejecucion: {Url}", url);
            return await _client.GetStringAsync(url, ct);
        }

        public async Task<string> GetReporteDetalleCargueArchivoGlosasAsync(int idCargueArchivo, CancellationToken ct = default)
        {
            string url = _aa + $"/api/CargueArchivo/GetReporteDetalleCargueArchivoGlosasAsync?idCargueArchivo={idCargueArchivo}";
            _logger.LogInformation("Requesting detalle: {Url}", url);
            var resp = await _client.PostAsync(url, new StringContent(string.Empty, Encoding.UTF8, "text/plain"), ct);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync(ct);
        }
    }
}
