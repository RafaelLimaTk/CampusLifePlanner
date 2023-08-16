using CampusLifePlanner.Domain.Entities.Base;
using System.Reflection.Metadata;

namespace CampusLifePlanner.Domain.Entities;

public class EnrollmentCourse : EntityBase
{
    private Guid _UserId;
    private Guid _CourseId;

    public Guid UserId
    {
        get { return _UserId; }
        private set { _UserId = value; }
    }

    public Guid CourseId
    {
        get { return _CourseId; }
        private set { _CourseId = value; }
    }

    public EnrollmentCourse() { }
}
