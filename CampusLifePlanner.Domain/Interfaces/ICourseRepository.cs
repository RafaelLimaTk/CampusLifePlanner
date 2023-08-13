using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.Domain.Interfaces.Base;

namespace CampusLifePlanner.Domain.Interfaces;

public interface ICourseRepository : IRepository<Course>
{
    bool ExistEvent(Guid id);
}
