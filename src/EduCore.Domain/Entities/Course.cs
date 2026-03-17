namespace EduCore.Domain.Entities;

public class Course : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Credits { get; set; }
    public int HoursPerWeek { get; set; }
    public int GradeLevelId { get; set; }

    public GradeLevel GradeLevel { get; set; } = null!;
    public ICollection<CourseAssignment> CourseAssignments { get; set; } = new List<CourseAssignment>();
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
