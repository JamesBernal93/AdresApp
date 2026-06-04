using AdresApp.Domain.Entities;
using AdresApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AdresApp.Infrastructure.Persistence.Repositories;

public class CargueArchivoRepository : ICargueArchivoRepository
{
    private readonly AppDbContext _context;

    public CargueArchivoRepository(AppDbContext context) => _context = context;

    public async Task<CargueArchivo?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.CarguesArchivo.FindAsync(new object[] { id }, cancellationToken);

    public async Task<IEnumerable<CargueArchivo>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        => await _context.CarguesArchivo.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<int> AgregarAsync(CargueArchivo cargueArchivo, CancellationToken cancellationToken = default)
    {
        await _context.CarguesArchivo.AddAsync(cargueArchivo, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return cargueArchivo.Id;
    }

    public async Task ActualizarAsync(CargueArchivo cargueArchivo, CancellationToken cancellationToken = default)
    {
        _context.CarguesArchivo.Update(cargueArchivo);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
