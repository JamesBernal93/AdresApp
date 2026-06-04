using AdresApp.Domain.Exceptions;
using AdresApp.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace AdresApp.Infrastructure.External;

public class AdresExternalService : IAdresExternalService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public AdresExternalService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["AdresService:BaseUrl"]
            ?? throw new InvalidOperationException("AdresService:BaseUrl no esta configurado.");
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

        var response = await _httpClient.PostAsync(
            $"{_baseUrl}/api/CargueArchivo/GuardarArchivoAsync", content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new ExternalServiceException($"Error al cargar archivo. HTTP {(int)response.StatusCode}: {error}");
        }

        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    public async Task<string> ConsultarEstadoCargueAsync(int idCargueArchivo, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsync(
            $"{_baseUrl}/api/CargueArchivo/GetRerpoteCargueArchivo?idCargueArchivo={idCargueArchivo}",
            new StringContent(string.Empty), cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new ExternalServiceException($"Error al consultar estado. HTTP {(int)response.StatusCode}: {error}");
        }

        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
}
