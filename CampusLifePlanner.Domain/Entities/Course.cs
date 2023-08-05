using CampusLifePlanner.Domain.Entities.Base;
using CampusLifePlanner.Domain.ExtensionMethods;

namespace CampusLifePlanner.Domain.Entities;

public class Course : EntityBase
{
    private string? _Name;
    private ICollection<Event> _Events = new List<Event>();

    public Course() { }

    public Course(string name)
    {
        name.EnsureNotNullOrEmpty(nameof(name));
        _Name = name;
    }

    public string Name
    {
        get => _Name;
        private set => value.EnsureNotNullOrEmpty(nameof(value));
    }

    public IEnumerable<Event> Events
    {
        get => _Events;
        private set => _Events = (ICollection<Event>)value;
    }
}
