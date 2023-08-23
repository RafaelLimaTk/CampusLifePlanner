namespace CampusLifePlanner.Application.DTOs;

public class EventLogDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid EventId { get; set; }
    public bool Completed { get; set; }
}
