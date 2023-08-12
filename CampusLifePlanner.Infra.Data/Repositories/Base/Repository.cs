using CampusLifePlanner.Domain.Entities.Base;
using CampusLifePlanner.Domain.Interfaces.Base;
using CampusLifePlanner.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CampusLifePlanner.Infra.Data.Repositories.Base;

public class Repository<T> : IRepository<T> where T : class, IEntity
{
    private readonly ApplicationDbContext _context;
    public DbSet<T> Entities { get; }
    public Repository(ApplicationDbContext context)
    {
        _context = context;
        Entities = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Entities.AsNoTracking().ToListAsync();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await Entities.FindAsync(id);
    }

    public async Task CreateAsync(T entity)
    {
        Entities.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        T entityToDelete = await Entities.FindAsync(id);
        Entities.Remove(entityToDelete);
        await _context.SaveChangesAsync();
    }
}
