using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AdresApp.Application.DTOs;
using AdresApp.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AdresApp.Infrastructure.Auth;

public class JwtAuthService : IAuthService
{
    private readonly IConfiguration _configuration;

    private readonly Dictionary<string, string> _usuarios = new()
    {
        { "admin", "Admin123!" },
        { "adres", "Adres123!" }
    };

    public JwtAuthService(IConfiguration configuration) => _configuration = configuration;

    public Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        if (!_usuarios.TryGetValue(request.Usuario, out var pwd) || pwd != request.Password)
            return Task.FromResult<LoginResponseDto?>(null);

        var jwt = _configuration.GetSection("JwtSettings");
        var secretKey = jwt["SecretKey"] ?? throw new InvalidOperationException("JwtSettings:SecretKey no configurado.");
        var expiracion = DateTime.UtcNow.AddMinutes(int.Parse(jwt["ExpirationMinutes"] ?? "60"));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[] { new Claim(ClaimTypes.Name, request.Usuario), new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) };

        var token = new JwtSecurityToken(jwt["Issuer"], jwt["Audience"], claims, expires: expiracion, signingCredentials: creds);
        return Task.FromResult<LoginResponseDto?>(new LoginResponseDto(new JwtSecurityTokenHandler().WriteToken(token), expiracion));
    }
}
