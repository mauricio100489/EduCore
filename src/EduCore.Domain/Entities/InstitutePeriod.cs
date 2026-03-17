namespace EduCore.Domain.Entities;

public class InstitutePeriod : BaseEntity
{
    public string PeriodName { get; set; } = string.Empty; // e.g. "2025-2026"
    public int InstituteId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? PromotionName { get; set; }
    public bool IsCurrent { get; set; }

    // Navigation
    public Institute Institute { get; set; } = null!;
    public ICollection<InstituteGrade> Grades { get; set; } = new List<InstituteGrade>();
    public ICollection<InstituteShift> Shifts { get; set; } = new List<InstituteShift>();
    public ICollection<InstituteSection> Sections { get; set; } = new List<InstituteSection>();
}
