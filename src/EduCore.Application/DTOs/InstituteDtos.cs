namespace EduCore.Application.DTOs;

public class InstituteDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string? Rtn { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Slogan { get; set; }
    public string? Address { get; set; }
    public string? Phones { get; set; }
    public string? Director { get; set; }
    public string? Secretary { get; set; }
    public bool IsActive { get; set; } = true;
}

public class InstitutePeriodDto
{
    public int Id { get; set; }
    public string PeriodName { get; set; } = string.Empty;
    public int InstituteId { get; set; }
    public string? InstituteName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? PromotionName { get; set; }
    public bool IsCurrent { get; set; }
    public bool IsActive { get; set; } = true;
}

public class InstituteGradeDto
{
    public int Id { get; set; }
    public int InstituteId { get; set; }
    public int PeriodId { get; set; }
    public string? InstituteName { get; set; }
    public string? PeriodName { get; set; }
    public string GradeName { get; set; } = string.Empty;
    public string? OfficialGrade { get; set; }
    public string? OfficialModality { get; set; }
    public int Order { get; set; }
    public decimal? EnrollmentFee { get; set; }
    public decimal? MonthlyFee12 { get; set; }
    public decimal? MonthlyFee10 { get; set; }
    public decimal? MaterialsFee { get; set; }
    public decimal? OtherFees { get; set; }
    public string? EvaluationType { get; set; }
    public string? EmailSender { get; set; }
    public string? EmailName { get; set; }
    public string? SmtpServer { get; set; }
    public int? SmtpPort { get; set; }
    public string? EmailTitle { get; set; }
    public string? CounselorName { get; set; }
    public int? EducationLevelId { get; set; }
    public bool IsActive { get; set; } = true;
}

public class InstituteShiftDto
{
    public int Id { get; set; }
    public string ShiftName { get; set; } = string.Empty;
    public int InstituteId { get; set; }
    public int PeriodId { get; set; }
    public string? InstituteName { get; set; }
    public string? PeriodName { get; set; }
    public int? Order { get; set; }
    public bool IsActive { get; set; } = true;
}

public class InstituteSectionDto
{
    public int Id { get; set; }
    public string SectionName { get; set; } = string.Empty;
    public int InstituteId { get; set; }
    public int PeriodId { get; set; }
    public string? InstituteName { get; set; }
    public string? PeriodName { get; set; }
    public int? Order { get; set; }
    public bool IsActive { get; set; } = true;
}
