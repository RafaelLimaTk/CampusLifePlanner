using CampusLifePlanner.Application.DTOs;

namespace CampusLifePlanner.WebUI.ViewModels;

public class EventViewModel
{
    public IEnumerable<EventDto> TodayEvents { get; set; }
    public IEnumerable<EventDto> NextDayEvents { get; set; }
}
