# AdresApp - API Cargue de Archivos ADRES

## Estructura del proyecto

```
AdresApp/
├── AdresApp.sln
├── src/
│   ├── AdresApp.Domain/               <- Entidades, interfaces, excepciones
│   ├── AdresApp.Application/          <- Servicios, DTOs, validaciones
│   ├── AdresApp.Infrastructure/       <- EF Core, repositorio, ADRES HTTP, JWT
│   └── AdresApp.API/                  <- Controllers, middleware, Swagger
└── tests/
    └── AdresApp.Domain.Tests/         <- Pruebas unitarias xUnit
```

## Requisitos
- .NET 8 SDK
- SQL Server (local o Docker)

## Configuracion
Editar `src/AdresApp.API/appsettings.json`:
- ConnectionStrings:DefaultConnection -> cadena de SQL Server
- JwtSettings:SecretKey -> clave de al menos 32 caracteres
- AdresService:BaseUrl -> URL del servicio ADRES (parametrica)

## Ejecucion
```bash
dotnet restore
dotnet run --project src/AdresApp.API
# Swagger en: https://localhost:{puerto}/swagger
```

## Pruebas
```bash
dotnet test tests/AdresApp.Domain.Tests
```

## Credenciales demo
| Usuario | Password  |
|---------|-----------|
| admin   | Admin123! |
| adres   | Adres123! |

## Endpoints
| Metodo | Ruta                          | Auth | Descripcion              |
|--------|-------------------------------|------|--------------------------|
| POST   | /api/auth/login               | No   | Obtener token JWT        |
| GET    | /api/cargueArchivo            | JWT  | Listar cargues           |
| POST   | /api/cargueArchivo/cargar     | JWT  | Cargar archivo a ADRES   |
| GET    | /api/cargueArchivo/{id}/estado| JWT  | Consultar estado en ADRES|
