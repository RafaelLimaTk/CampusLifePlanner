using CampusLifePlanner.Domain.Entities.Base;

namespace CampusLifePlanner.Domain.Entities
{
    public class User : EntityBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
