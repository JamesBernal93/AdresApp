using AdresApp.Application.DTOs;
using AdresApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdresApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService) => _authService = authService;

    /// <summary>Genera un token JWT para autenticacion.</summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponseDto<LoginResponseDto>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<object>), 401)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);
        if (result is null) return Unauthorized(ApiResponseDto<object>.Error("Credenciales invalidas."));
        return Ok(ApiResponseDto<LoginResponseDto>.Ok(result, "Login exitoso."));
    }
}
