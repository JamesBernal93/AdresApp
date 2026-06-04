using AdresApp.Application.DTOs;
using AdresApp.Application.Services;
using AdresApp.Application.Validators;
using AdresApp.Domain.Entities;
using AdresApp.Domain.Exceptions;
using AdresApp.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace AdresApp.Domain.Tests;

public class CargueArchivoEntityTests
{
    [Fact]
    public void Crear_ConDatosValidos_RetornaEntidad()
    {
        var e = CargueArchivo.Crear(111, 4645, "archivo.xlsx");
        e.IdArchivo.Should().Be(111);
        e.IdPrestador.Should().Be(4645);
        e.Estado.Should().Be("PENDIENTE");
    }

    [Theory]
    [InlineData(0, 1, "archivo.xlsx")]
    [InlineData(1, 0, "archivo.xlsx")]
    [InlineData(1, 1, "")]
    public void Crear_ConDatosInvalidos_LanzaExcepcion(int idArchivo, int idPrestador, string nombre)
    {
        var act = () => CargueArchivo.Crear(idArchivo, idPrestador, nombre);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void RegistrarRespuestaCargue_CambiaEstadoACargado()
    {
        var e = CargueArchivo.Crear(1, 1, "test.xlsx");
        e.RegistrarRespuestaCargue("{\"ok\":true}");
        e.Estado.Should().Be("CARGADO");
        e.FechaActualizacion.Should().NotBeNull();
    }

    [Fact]
    public void MarcarError_CambiaEstadoAError()
    {
        var e = CargueArchivo.Crear(1, 1, "test.xlsx");
        e.MarcarError("Timeout");
        e.Estado.Should().Be("ERROR");
        e.MensajeError.Should().Be("Timeout");
    }

    [Fact]
    public void RegistrarRespuestaCargue_ConNull_LanzaExcepcion()
    {
        var e = CargueArchivo.Crear(1, 1, "test.xlsx");
        var act = () => e.RegistrarRespuestaCargue(null!);
        act.Should().Throw<ArgumentNullException>();
    }
}

public class CargueArchivoValidatorTests
{
    private readonly CargueArchivoRequestValidator _validator = new();

    [Fact]
    public void Validar_ConDatosValidos_EsValido()
    {
        var result = _validator.Validate(new CargueArchivoRequestDto(111, 4645));
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(-1, 1)]
    public void Validar_ConDatosInvalidos_NoEsValido(int idArchivo, int idPrestador)
    {
        var result = _validator.Validate(new CargueArchivoRequestDto(idArchivo, idPrestador));
        result.IsValid.Should().BeFalse();
    }
}

public class CargueArchivoServiceTests
{
    private readonly Mock<ICargueArchivoRepository> _repoMock = new();
    private readonly Mock<IAdresExternalService> _adresMock = new();

    [Fact]
    public async Task ObtenerTodos_RetornaLista()
    {
        var lista = new List<CargueArchivo> { CargueArchivo.Crear(1, 100, "a.xlsx") };
        _repoMock.Setup(r => r.ObtenerTodosAsync(It.IsAny<CancellationToken>())).ReturnsAsync(lista);

        var service = new CargueArchivoService(_repoMock.Object, _adresMock.Object);
        var resultado = await service.ObtenerTodosAsync();
        resultado.Should().HaveCount(1);
    }

    [Fact]
    public async Task ConsultarEstado_ConIdInexistente_LanzaNotFoundException()
    {
        _repoMock.Setup(r => r.ObtenerPorIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((CargueArchivo?)null);
        var service = new CargueArchivoService(_repoMock.Object, _adresMock.Object);
        await Assert.ThrowsAsync<NotFoundException>(() => service.ConsultarEstadoAsync(99));
    }

    [Fact]
    public async Task CargarArchivo_CuandoAdresFalla_LanzaExternalServiceException()
    {
        _repoMock.Setup(r => r.AgregarAsync(It.IsAny<CargueArchivo>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _repoMock.Setup(r => r.ActualizarAsync(It.IsAny<CargueArchivo>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _adresMock.Setup(a => a.CargarArchivoAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("Timeout"));

        var service = new CargueArchivoService(_repoMock.Object, _adresMock.Object);
        await Assert.ThrowsAsync<ExternalServiceException>(
            () => service.CargarArchivoAsync(new CargueArchivoRequestDto(1, 1), Stream.Null, "test.xlsx"));
    }
}
