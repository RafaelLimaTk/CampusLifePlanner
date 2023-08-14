using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces.Base;
using CampusLifePlanner.Domain.Entities;

namespace CampusLifePlanner.Application.Interfaces
{
    public interface IUserService : IService<UserDto, User>
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(Guid id);
    }
}
