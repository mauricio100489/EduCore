using EduCore.Application.DTOs;

namespace EduCore.Application.Interfaces;

public interface IInstituteService
{
    Task<IEnumerable<InstituteDto>> GetAllAsync();
    Task<InstituteDto?> GetByIdAsync(int id);
    Task<InstituteDto> CreateAsync(InstituteDto dto);
    Task UpdateAsync(InstituteDto dto);
    Task DeleteAsync(int id);
}

public interface IInstitutePeriodService
{
    Task<IEnumerable<InstitutePeriodDto>> GetAllAsync();
    Task<IEnumerable<InstitutePeriodDto>> GetByInstituteAsync(int instituteId);
    Task<InstitutePeriodDto?> GetByIdAsync(int id);
    Task<InstitutePeriodDto> CreateAsync(InstitutePeriodDto dto);
    Task UpdateAsync(InstitutePeriodDto dto);
    Task DeleteAsync(int id);
}

public interface IInstituteGradeService
{
    Task<IEnumerable<InstituteGradeDto>> GetAllAsync();
    Task<IEnumerable<InstituteGradeDto>> GetByInstituteAndPeriodAsync(int instituteId, int periodId);
    Task<InstituteGradeDto?> GetByIdAsync(int id);
    Task<InstituteGradeDto> CreateAsync(InstituteGradeDto dto);
    Task UpdateAsync(InstituteGradeDto dto);
    Task DeleteAsync(int id);
}

public interface IInstituteShiftService
{
    Task<IEnumerable<InstituteShiftDto>> GetAllAsync();
    Task<IEnumerable<InstituteShiftDto>> GetByInstituteAndPeriodAsync(int instituteId, int periodId);
    Task<InstituteShiftDto?> GetByIdAsync(int id);
    Task<InstituteShiftDto> CreateAsync(InstituteShiftDto dto);
    Task UpdateAsync(InstituteShiftDto dto);
    Task DeleteAsync(int id);
}

public interface IInstituteSectionService
{
    Task<IEnumerable<InstituteSectionDto>> GetAllAsync();
    Task<IEnumerable<InstituteSectionDto>> GetByInstituteAndPeriodAsync(int instituteId, int periodId);
    Task<InstituteSectionDto?> GetByIdAsync(int id);
    Task<InstituteSectionDto> CreateAsync(InstituteSectionDto dto);
    Task UpdateAsync(InstituteSectionDto dto);
    Task DeleteAsync(int id);
}
