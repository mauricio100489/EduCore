using EduCore.Domain.Enums;

namespace EduCore.Application.DTOs;

public class StudentDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string? IdentificationNumber { get; set; }
    public string? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? BirthPlace { get; set; }
    public string? Address { get; set; }
    public string? Handedness { get; set; }
    public string? PhotoBase64 { get; set; }

    // Salud
    public string? MedicalConditions { get; set; }
    public bool? HasAllergies { get; set; }
    public string? AllergiesDescription { get; set; }

    // Familia
    public string? HasSiblings { get; set; }
    public string? SiblingsCount { get; set; }
    public string? SiblingsGrades { get; set; }

    // Observaciones
    public string? ParentObservations { get; set; }
    public string? ReferralEmails { get; set; }

    public bool IsActive { get; set; }
    public string FullName => $"{FirstName} {LastName}";

    // Responsables
    public List<StudentGuardianDto> Guardians { get; set; } = new();
}

public class StudentGuardianDto
{
    public int Id { get; set; }
    public GuardianType Type { get; set; }
    public string? IdentificationNumber { get; set; }
    public string? FullName { get; set; }
    public string? Phones { get; set; }
    public string? Email { get; set; }
}

public class EnrollmentDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int GradeLevelId { get; set; }
    public string GradeLevelName { get; set; } = string.Empty;
    public int AcademicPeriodId { get; set; }
    public string AcademicPeriodName { get; set; } = string.Empty;

    // Datos academicos
    public string? Section { get; set; }
    public string? Shift { get; set; }
    public string? ListNumber { get; set; }
    public DateTime? EnrollmentDate { get; set; }
    public string? EnrollmentMethod { get; set; }
    public EnrollmentStatus Status { get; set; }
    public string? Notes { get; set; }

    // Instituto de procedencia
    public string? PreviousInstitute { get; set; }
    public string? PreviousInstituteAddress { get; set; }
    public string? PreviousGrade { get; set; }
    public string? PreviousShift { get; set; }

    // Becas y descuentos
    public bool? HasScholarship { get; set; }
    public decimal? ScholarshipAmount { get; set; }
    public bool? FullScholarship { get; set; }
    public bool? HasEnrollmentDiscount { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? LevelingFee { get; set; }
    public string? PaymentForm { get; set; }

    // Flags
    public bool? IsWithdrawn { get; set; }
    public bool? IsAuditor { get; set; }
    public bool? IsTransfer { get; set; }
    public bool? IsReentry { get; set; }
    public bool? AcceptedTerms { get; set; }
    public bool? ReportCardPrinted { get; set; }

    // Observaciones
    public string? GradeObservations { get; set; }
    public int? LegacyEnrollmentId { get; set; }
}

public class TeacherDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string IdentificationNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Specialty { get; set; }
    public string? Degree { get; set; }
    public DateTime HireDate { get; set; }
    public string? PhotoUrl { get; set; }
    public bool IsActive { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}

public class CourseDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Credits { get; set; }
    public int HoursPerWeek { get; set; }
    public int GradeLevelId { get; set; }
    public string GradeLevelName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class GradeLevelDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Order { get; set; }
    public string Section { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class AcademicPeriodDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCurrent { get; set; }
    public bool IsActive { get; set; }
}

public class GradeDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int AcademicPeriodId { get; set; }
    public string AcademicPeriodName { get; set; } = string.Empty;
    public GradeType Type { get; set; }
    public string? Description { get; set; }
    public decimal Score { get; set; }
    public decimal MaxScore { get; set; }
    public decimal Percentage => MaxScore > 0 ? (Score / MaxScore) * 100 : 0;
    public DateTime GradeDate { get; set; }
    public string? Observations { get; set; }
}

public class InvoiceDto
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int AcademicPeriodId { get; set; }
    public string AcademicPeriodName { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Discount { get; set; }
    public decimal Total => Subtotal + Tax - Discount;
    public PaymentStatus Status { get; set; }
    public string? Notes { get; set; }
    public List<InvoiceDetailDto> Details { get; set; } = new();
    public List<PaymentDto> Payments { get; set; } = new();
}

public class InvoiceDetailDto
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public string Concept { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total => Quantity * UnitPrice;
}

public class PaymentDto
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentMethod Method { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Notes { get; set; }
}

public class DashboardDto
{
    public int TotalStudents { get; set; }
    public int ActiveEnrollments { get; set; }
    public int TotalTeachers { get; set; }
    public int TotalCourses { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal PendingPayments { get; set; }
    public List<MonthlyRevenueDto> MonthlyRevenue { get; set; } = new();
    public List<EnrollmentsByGradeDto> EnrollmentsByGrade { get; set; } = new();
}

public class MonthlyRevenueDto
{
    public string Month { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class EnrollmentsByGradeDto
{
    public string GradeLevel { get; set; } = string.Empty;
    public int Count { get; set; }
}
