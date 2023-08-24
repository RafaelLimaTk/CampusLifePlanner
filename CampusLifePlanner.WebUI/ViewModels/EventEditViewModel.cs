using CampusLifePlanner.Application.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CampusLifePlanner.WebUI.ViewModels;

public class EventEditViewModel
{
    public EventDto Event { get; set; }
    public SelectList Courses { get; set; }
}
