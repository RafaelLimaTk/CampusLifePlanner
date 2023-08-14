using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Application.Services.Base;
using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.Domain.Interfaces;

namespace CampusLifePlanner.Application.Services
{
    public class UserService : GenericService<UserDto, User>, IUserService
    {
        private readonly IUserRepository repository;

        public UserService(IUserRepository repository, IMapper mapper) : base(repository, mapper)
        {
            this.repository = repository;
        }

        public async Task<User> GetById(Guid id)
         => await repository.GetByIdAsync(id);

        public async Task<IEnumerable<User>> GetAll()
         => await repository.GetAllAsync();
    }
}
