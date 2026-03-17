namespace EduCore.Domain.Entities;

public class Institute : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string? Rtn { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Slogan { get; set; }
    public string? Address { get; set; }
    public string? Phones { get; set; }
    public string? Director { get; set; }
    public byte[]? DirectorSignature { get; set; }
    public string? Secretary { get; set; }
    public byte[]? SecretarySignature { get; set; }
    public byte[]? Logo { get; set; }

    // Navigation
    public ICollection<InstituteGrade> Grades { get; set; } = new List<InstituteGrade>();
    public ICollection<InstituteShift> Shifts { get; set; } = new List<InstituteShift>();
    public ICollection<InstituteSection> Sections { get; set; } = new List<InstituteSection>();
    public ICollection<InstitutePeriod> Periods { get; set; } = new List<InstitutePeriod>();
}
