using EduCore.Domain.Enums;

namespace EduCore.Domain.Entities;

public class Grade : BaseEntity
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public int AcademicPeriodId { get; set; }
    public GradeType Type { get; set; }
    public string? Description { get; set; }
    public decimal Score { get; set; }
    public decimal MaxScore { get; set; } = 100;
    public decimal Percentage => MaxScore > 0 ? (Score / MaxScore) * 100 : 0;
    public DateTime GradeDate { get; set; } = DateTime.UtcNow;
    public string? Observations { get; set; }

    public Student Student { get; set; } = null!;
    public Course Course { get; set; } = null!;
    public AcademicPeriod AcademicPeriod { get; set; } = null!;
}
