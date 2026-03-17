namespace EduCore.Domain.Entities;

public class CourseAssignment : BaseEntity
{
    public int TeacherId { get; set; }
    public int CourseId { get; set; }
    public int AcademicPeriodId { get; set; }
    public string? Schedule { get; set; }
    public string? Classroom { get; set; }

    public Teacher Teacher { get; set; } = null!;
    public Course Course { get; set; } = null!;
    public AcademicPeriod AcademicPeriod { get; set; } = null!;
}
