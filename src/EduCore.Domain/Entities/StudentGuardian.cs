using EduCore.Domain.Enums;

namespace EduCore.Domain.Entities;

public class StudentGuardian : BaseEntity
{
    public int StudentId { get; set; }
    public GuardianType Type { get; set; }
    public string? IdentificationNumber { get; set; }
    public string? FullName { get; set; }
    public string? Phones { get; set; }
    public string? Email { get; set; }

    public Student Student { get; set; } = null!;
}
