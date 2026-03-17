using EduCore.Application.DTOs;
using EduCore.Application.Interfaces;
using EduCore.Domain.Entities;
using EduCore.Domain.Interfaces;

namespace EduCore.Application.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IUnitOfWork _uow;
    public EnrollmentService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<EnrollmentDto>> GetAllAsync()
    {
        var items = await _uow.Enrollments.GetAllAsync();
        return items.Select(MapToDto);
    }

    public async Task<EnrollmentDto?> GetByIdAsync(int id)
    {
        var e = await _uow.Enrollments.GetByIdAsync(id);
        return e is null ? null : MapToDto(e);
    }

    public async Task<EnrollmentDto> CreateAsync(EnrollmentDto dto)
    {
        var entity = MapToEntity(dto);
        var created = await _uow.Enrollments.AddAsync(entity);
        await _uow.SaveChangesAsync();
        return MapToDto(created);
    }

    public async Task UpdateAsync(EnrollmentDto dto)
    {
        var entity = await _uow.Enrollments.GetByIdAsync(dto.Id)
            ?? throw new KeyNotFoundException($"Enrollment {dto.Id} not found");

        entity.StudentId = dto.StudentId;
        entity.GradeLevelId = dto.GradeLevelId;
        entity.AcademicPeriodId = dto.AcademicPeriodId;
        entity.Section = dto.Section;
        entity.Shift = dto.Shift;
        entity.ListNumber = dto.ListNumber;
        entity.EnrollmentDate = dto.EnrollmentDate;
        entity.EnrollmentMethod = dto.EnrollmentMethod;
        entity.Status = dto.Status;
        entity.Notes = dto.Notes;
        entity.PreviousInstitute = dto.PreviousInstitute;
        entity.PreviousInstituteAddress = dto.PreviousInstituteAddress;
        entity.PreviousGrade = dto.PreviousGrade;
        entity.PreviousShift = dto.PreviousShift;
        entity.HasScholarship = dto.HasScholarship;
        entity.ScholarshipAmount = dto.ScholarshipAmount;
        entity.FullScholarship = dto.FullScholarship;
        entity.HasEnrollmentDiscount = dto.HasEnrollmentDiscount;
        entity.DiscountAmount = dto.DiscountAmount;
        entity.LevelingFee = dto.LevelingFee;
        entity.PaymentForm = dto.PaymentForm;
        entity.IsWithdrawn = dto.IsWithdrawn;
        entity.IsAuditor = dto.IsAuditor;
        entity.IsTransfer = dto.IsTransfer;
        entity.IsReentry = dto.IsReentry;
        entity.AcceptedTerms = dto.AcceptedTerms;
        entity.ReportCardPrinted = dto.ReportCardPrinted;
        entity.GradeObservations = dto.GradeObservations;
        entity.UpdatedAt = DateTime.UtcNow;

        await _uow.Enrollments.UpdateAsync(entity);
        await _uow.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _uow.Enrollments.DeleteAsync(id);
        await _uow.SaveChangesAsync();
    }

    public async Task<IEnumerable<EnrollmentDto>> GetByStudentAsync(int studentId)
    {
        var items = await _uow.Enrollments.FindAsync(e => e.StudentId == studentId);
        return items.Select(MapToDto);
    }

    public async Task<IEnumerable<EnrollmentDto>> GetByPeriodAsync(int periodId)
    {
        var items = await _uow.Enrollments.FindAsync(e => e.AcademicPeriodId == periodId);
        return items.Select(MapToDto);
    }

    private static EnrollmentDto MapToDto(Enrollment e) => new()
    {
        Id = e.Id,
        StudentId = e.StudentId,
        StudentName = e.Student?.FullName ?? "",
        GradeLevelId = e.GradeLevelId,
        GradeLevelName = e.GradeLevel?.Name ?? "",
        AcademicPeriodId = e.AcademicPeriodId,
        AcademicPeriodName = e.AcademicPeriod?.Name ?? "",
        Section = e.Section,
        Shift = e.Shift,
        ListNumber = e.ListNumber,
        EnrollmentDate = e.EnrollmentDate,
        EnrollmentMethod = e.EnrollmentMethod,
        Status = e.Status,
        Notes = e.Notes,
        PreviousInstitute = e.PreviousInstitute,
        PreviousInstituteAddress = e.PreviousInstituteAddress,
        PreviousGrade = e.PreviousGrade,
        PreviousShift = e.PreviousShift,
        HasScholarship = e.HasScholarship,
        ScholarshipAmount = e.ScholarshipAmount,
        FullScholarship = e.FullScholarship,
        HasEnrollmentDiscount = e.HasEnrollmentDiscount,
        DiscountAmount = e.DiscountAmount,
        LevelingFee = e.LevelingFee,
        PaymentForm = e.PaymentForm,
        IsWithdrawn = e.IsWithdrawn,
        IsAuditor = e.IsAuditor,
        IsTransfer = e.IsTransfer,
        IsReentry = e.IsReentry,
        AcceptedTerms = e.AcceptedTerms,
        ReportCardPrinted = e.ReportCardPrinted,
        GradeObservations = e.GradeObservations,
        LegacyEnrollmentId = e.LegacyEnrollmentId
    };

    private static Enrollment MapToEntity(EnrollmentDto d) => new()
    {
        StudentId = d.StudentId,
        GradeLevelId = d.GradeLevelId,
        AcademicPeriodId = d.AcademicPeriodId,
        Section = d.Section,
        Shift = d.Shift,
        ListNumber = d.ListNumber,
        EnrollmentDate = d.EnrollmentDate,
        EnrollmentMethod = d.EnrollmentMethod,
        Status = d.Status,
        Notes = d.Notes,
        PreviousInstitute = d.PreviousInstitute,
        PreviousInstituteAddress = d.PreviousInstituteAddress,
        PreviousGrade = d.PreviousGrade,
        PreviousShift = d.PreviousShift,
        HasScholarship = d.HasScholarship,
        ScholarshipAmount = d.ScholarshipAmount,
        FullScholarship = d.FullScholarship,
        HasEnrollmentDiscount = d.HasEnrollmentDiscount,
        DiscountAmount = d.DiscountAmount,
        LevelingFee = d.LevelingFee,
        PaymentForm = d.PaymentForm,
        IsWithdrawn = d.IsWithdrawn,
        IsAuditor = d.IsAuditor,
        IsTransfer = d.IsTransfer,
        IsReentry = d.IsReentry,
        AcceptedTerms = d.AcceptedTerms,
        ReportCardPrinted = d.ReportCardPrinted,
        GradeObservations = d.GradeObservations,
        LegacyEnrollmentId = d.LegacyEnrollmentId
    };
}
