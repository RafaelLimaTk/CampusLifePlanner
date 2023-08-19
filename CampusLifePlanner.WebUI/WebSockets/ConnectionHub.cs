using CampusLifePlanner.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CampusLifePlanner.WebUI.WebSockets;

public class ConnectionHub : Hub
{
    private readonly IEventService _eventService;
    public ConnectionHub(IEventService eventService)
    {
        _eventService = eventService;
    }

    public async Task ShareEvent(string eventIdString, string targetCourseIdString)
    {
        Guid eventId = Guid.Parse(eventIdString);
        Guid targetCourseId = Guid.Parse(targetCourseIdString);
        var eventCloneSuccess = await _eventService.ShareEvent(eventId, targetCourseId);
        if (eventCloneSuccess)
        {
            await Clients.Caller.SendAsync("EventShared", eventId, targetCourseId);
        }
    }

    public override Task OnConnectedAsync()
    {
        var x = Context.ConnectionId;

        return base.OnConnectedAsync();
    }
}
