using CampusLifePlanner.Domain.Entities.Base;

namespace CampusLifePlanner.Domain.Entities;

public class CodeGeneration : EntityBase
{
    public string Code { get; set; }
    public Guid UserId { get; set; }
}
