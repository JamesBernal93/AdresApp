using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AdresApp.Application.DTOs;
using AdresApp.Domain.Interfaces;

namespace AdresApp.Application.Services
{
    public class CargueEstadoService
    {
        private readonly IAdresExternalService _adres;

        public CargueEstadoService(IAdresExternalService adres)
        {
            _adres = adres;
        }

        public async Task<EstadoCargueResponseDto> ObtenerEstadoAsync(int idCargue, CancellationToken ct = default)
        {
            var ejecucionJson = await _adres.GetReporteEjecucionAsync(idCargue, ct);
            var ejecucion = JsonSerializer.Deserialize<ReporteEjecucionDto>(ejecucionJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                           ?? throw new InvalidOperationException("Respuesta inválida de reporte ejecucion.");

            var result = new EstadoCargueResponseDto { ReporteEjecucion = ejecucion };

            if (ejecucion.Porcentaje >= 100)
            {
                var detalleRaw = await _adres.GetReporteDetalleCargueArchivoGlosasAsync(idCargue, ct);
                var detalles = ParseDetalles(detalleRaw);
                result.Detalles = detalles;
            }

            return result;
        }

        private IEnumerable<DetalleRegistroDto> ParseDetalles(string raw)
        {
            var lines = raw.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var regNumber = 0;
                var codigo = string.Empty;
                var mensaje = string.Empty;

                var prefix = "Reg ";
                if (line.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    var afterReg = line.Substring(prefix.Length);
                    var colonIdx = afterReg.IndexOf(':');
                    if (colonIdx > 0 && int.TryParse(afterReg.Substring(0, colonIdx).Trim(), out var r))
                        regNumber = r;
                }

                var lastDash = line.LastIndexOf('-');
                if (lastDash > 0 && lastDash + 1 < line.Length)
                {
                    mensaje = line.Substring(lastDash + 1).Trim().Trim('"');
                    var beforeDash = line.Substring(0, lastDash);
                    var tokens = beforeDash.Split(new[] { ',', '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tokens.Length > 0)
                        codigo = tokens[tokens.Length - 1].Trim().Trim('"');
                }

                yield return new DetalleRegistroDto
                {
                    RegNumber = regNumber,
                    Raw = line,
                    Codigo = codigo,
                    Mensaje = mensaje
                };
            }
        }
    }
}
