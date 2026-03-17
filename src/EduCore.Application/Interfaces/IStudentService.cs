using EduCore.Application.DTOs;

namespace EduCore.Application.Interfaces;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> GetAllAsync();
    Task<StudentDto?> GetByIdAsync(int id);
    Task<StudentDto> CreateAsync(StudentDto dto);
    Task UpdateAsync(StudentDto dto);
    Task DeleteAsync(int id);
    Task<IEnumerable<StudentDto>> SearchAsync(string term);
}

public interface ITeacherService
{
    Task<IEnumerable<TeacherDto>> GetAllAsync();
    Task<TeacherDto?> GetByIdAsync(int id);
    Task<TeacherDto> CreateAsync(TeacherDto dto);
    Task UpdateAsync(TeacherDto dto);
    Task DeleteAsync(int id);
}

public interface ICourseService
{
    Task<IEnumerable<CourseDto>> GetAllAsync();
    Task<CourseDto?> GetByIdAsync(int id);
    Task<CourseDto> CreateAsync(CourseDto dto);
    Task UpdateAsync(CourseDto dto);
    Task DeleteAsync(int id);
}

public interface IGradeLevelService
{
    Task<IEnumerable<GradeLevelDto>> GetAllAsync();
    Task<GradeLevelDto?> GetByIdAsync(int id);
    Task<GradeLevelDto> CreateAsync(GradeLevelDto dto);
    Task UpdateAsync(GradeLevelDto dto);
    Task DeleteAsync(int id);
}

public interface IAcademicPeriodService
{
    Task<IEnumerable<AcademicPeriodDto>> GetAllAsync();
    Task<AcademicPeriodDto?> GetByIdAsync(int id);
    Task<AcademicPeriodDto> CreateAsync(AcademicPeriodDto dto);
    Task UpdateAsync(AcademicPeriodDto dto);
    Task DeleteAsync(int id);
}

public interface IEnrollmentService
{
    Task<IEnumerable<EnrollmentDto>> GetAllAsync();
    Task<EnrollmentDto?> GetByIdAsync(int id);
    Task<EnrollmentDto> CreateAsync(EnrollmentDto dto);
    Task UpdateAsync(EnrollmentDto dto);
    Task DeleteAsync(int id);
    Task<IEnumerable<EnrollmentDto>> GetByStudentAsync(int studentId);
    Task<IEnumerable<EnrollmentDto>> GetByPeriodAsync(int periodId);
}

public interface IGradeService
{
    Task<IEnumerable<GradeDto>> GetAllAsync();
    Task<GradeDto?> GetByIdAsync(int id);
    Task<GradeDto> CreateAsync(GradeDto dto);
    Task UpdateAsync(GradeDto dto);
    Task DeleteAsync(int id);
    Task<IEnumerable<GradeDto>> GetByStudentAsync(int studentId);
    Task<IEnumerable<GradeDto>> GetByStudentAndCourseAsync(int studentId, int courseId);
}

public interface IInvoiceService
{
    Task<IEnumerable<InvoiceDto>> GetAllAsync();
    Task<InvoiceDto?> GetByIdAsync(int id);
    Task<InvoiceDto> CreateAsync(InvoiceDto dto);
    Task UpdateAsync(InvoiceDto dto);
    Task DeleteAsync(int id);
    Task<IEnumerable<InvoiceDto>> GetByStudentAsync(int studentId);
    Task<PaymentDto> AddPaymentAsync(PaymentDto dto);
}

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardAsync();
}
