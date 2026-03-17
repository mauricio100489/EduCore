namespace EduCore.Domain.Entities;

public class Teacher : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string IdentificationNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Specialty { get; set; }
    public string? Degree { get; set; }
    public DateTime HireDate { get; set; }
    public string? PhotoUrl { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public ICollection<CourseAssignment> CourseAssignments { get; set; } = new List<CourseAssignment>();
}
