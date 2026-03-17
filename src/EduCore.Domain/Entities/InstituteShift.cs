namespace EduCore.Domain.Entities;

public class InstituteShift : BaseEntity
{
    public string ShiftName { get; set; } = string.Empty; // jornada
    public int InstituteId { get; set; }
    public int PeriodId { get; set; }
    public int? Order { get; set; }

    // Navigation
    public Institute Institute { get; set; } = null!;
    public InstitutePeriod Period { get; set; } = null!;
}
