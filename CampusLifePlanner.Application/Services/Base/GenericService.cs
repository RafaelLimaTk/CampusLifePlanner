using AutoMapper;
using CampusLifePlanner.Application.Interfaces.Base;
using CampusLifePlanner.Domain.Entities.Base;
using CampusLifePlanner.Domain.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace CampusLifePlanner.Application.Services.Base;

public class GenericService<TDto, TEntity> : IService<TDto, TEntity> where TEntity : EntityBase
{
    private readonly IRepository<TEntity> _repository;
    private readonly IMapper _mapper;
    public GenericService(IRepository<TEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<TEntity> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateAsync(TDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);
        await _repository.CreateAsync(entity);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        await _repository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task BeginTransaction()
    {
        await _repository.BeginTransaction();
    }

    public async Task CommitTransaction()
    {
        await _repository.CommitTransaction();
    }

    public async Task RollbackTransaction()
    {
        await _repository.RollbackTransaction();
    }
}
