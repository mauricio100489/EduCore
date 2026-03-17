namespace EduCore.Domain.Entities;

public class InstituteSection : BaseEntity
{
    public string SectionName { get; set; } = string.Empty; // seccion
    public int InstituteId { get; set; }
    public int PeriodId { get; set; }
    public int? Order { get; set; }

    // Navigation
    public Institute Institute { get; set; } = null!;
    public InstitutePeriod Period { get; set; } = null!;
}
