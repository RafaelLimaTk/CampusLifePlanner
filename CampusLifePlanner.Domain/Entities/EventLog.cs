using CampusLifePlanner.Domain.Entities.Base;

namespace CampusLifePlanner.Domain.Entities;

public class EventLog : EntityBase
{
    private Guid _UserId;
    private Guid _EventId;
    private bool _Completed;
    private Event _Event;

    public EventLog() { }

    public Guid UserId
    {
        get { return _UserId; }
        set { _UserId = value; }
    }

    public Guid EventId
    {
        get { return _EventId; }
        set { _EventId = value; }
    }

    public bool Completed
    {
        get { return _Completed; }
        set { _Completed = value; }
    }

    public Event Event
    {
        get { return _Event; }
        set { _Event = value; }
    }
}
