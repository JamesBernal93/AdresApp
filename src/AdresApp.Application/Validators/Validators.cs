using AdresApp.Application.DTOs;
using FluentValidation;

namespace AdresApp.Application.Validators;

public class CargueArchivoRequestValidator : AbstractValidator<CargueArchivoRequestDto>
{
    public CargueArchivoRequestValidator()
    {
        RuleFor(x => x.IdArchivo).GreaterThan(0).WithMessage("IdArchivo debe ser mayor a cero.");
        RuleFor(x => x.IdPrestador).GreaterThan(0).WithMessage("IdPrestador debe ser mayor a cero.");
    }
}

public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Usuario).NotEmpty().WithMessage("El usuario es requerido.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("La contrasena es requerida.");
    }
}
