using CampusLifePlanner.Domain.Entities.Base;

namespace CampusLifePlanner.Application.Interfaces.Base;

public interface IService<TDto, TEntity> where TEntity : IEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> GetByIdAsync(Guid id);
    Task CreateAsync(TDto dto);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(Guid id);
}
