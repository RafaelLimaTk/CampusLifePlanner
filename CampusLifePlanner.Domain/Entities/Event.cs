using CampusLifePlanner.Domain.Entities.Base;
using CampusLifePlanner.Domain.ExtensionMethods;

namespace CampusLifePlanner.Domain.Entities;

public class Event : EntityBase
{
    private string? _Title;
    private string? _Description;
    private string? _Local;
    private DateTime _StartDate;
    private DateTime _EndDate;
    private Guid _CourseId;
    private Course? _Course;
    private string? _JobId;
    private readonly List<EventLog> _eventLogs = new List<EventLog>();
    public IReadOnlyCollection<EventLog> EventLogs => _eventLogs.AsReadOnly();


    private Event() { }

    public Event(string title, string description, string local, DateTime startDate, DateTime endDate, Course course, string jobId)
    {
        title.EnsureNotNullOrEmpty(nameof(title));
        local.EnsureNotNullOrEmpty(nameof(local));
        ValidateStartDate(startDate);
        ValidateEndDate(startDate, endDate);
        course.EnsureNotNull(nameof(course));

        _Title = title;
        _Description = description;
        _Local = local;
        _StartDate = startDate;
        _EndDate = endDate;
        _Course = course;
        _CourseId = course.Id;
        _JobId = jobId;
    }

    public Event(Event otherEvent, Guid targetCourseId)
    {
        _Title = otherEvent._Title;
        _Description = otherEvent._Description;
        _Local = otherEvent._Local;
        _StartDate = otherEvent._StartDate;
        _EndDate = otherEvent._EndDate;
        _CourseId = targetCourseId;
        _Course = otherEvent._Course;
    }

    public string Title
    {
        get => _Title;
        private set
        {
            value.EnsureNotNullOrEmpty(nameof(Title));
            _Title = value;
        }
    }
    public string Local
    {
        get => _Local;
        private set
        {
            value.EnsureNotNullOrEmpty(nameof(Local));
            _Local = value;
        }
    }

    public string Description
    {
        get => _Description;
        private set => _Description = value;
    }

    public DateTime StartDate
    {
        get => _StartDate;
        private set
        {
            ValidateStartDate(value);
            _StartDate = value;
        }
    }

    public DateTime EndDate
    {
        get => _EndDate;
        private set
        {
            ValidateEndDate(_StartDate, value);
            _EndDate = value;
        }
    }

    public Guid CourseId
    {
        get => _CourseId;
        private set
        {
            if (value == default)
                throw new ArgumentException("Course ID cannot be default", nameof(CourseId));
            _CourseId = value;
        }
    }

    public Course Course
    {
        get => _Course;
        private set => value.EnsureNotNull(nameof(Course));
    }

    public string JobId
    {
        get => _JobId;
        private set
        {
            _JobId = value;
        }
    }


    private void ValidateStartDate(DateTime startDate)
    {
        if (startDate == default)
            throw new ArgumentException("Start date cannot be default", nameof(StartDate));
    }

    private void ValidateEndDate(DateTime startDate, DateTime endDate)
    {
        if (endDate == default)
            throw new ArgumentException("End date cannot be default", nameof(EndDate));
        if (endDate < startDate)
            throw new ArgumentException("End date cannot be earlier than start date", nameof(EndDate));
    }
}
