using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.Domain.Interfaces;
using CampusLifePlanner.Infra.Data.Context;
using CampusLifePlanner.Infra.Data.Repositories.Base;

namespace CampusLifePlanner.Infra.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {

        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
