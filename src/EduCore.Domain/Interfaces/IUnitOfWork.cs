using EduCore.Domain.Entities;

namespace EduCore.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Student> Students { get; }
    IRepository<StudentGuardian> StudentGuardians { get; }
    IRepository<Teacher> Teachers { get; }
    IRepository<Course> Courses { get; }
    IRepository<GradeLevel> GradeLevels { get; }
    IRepository<AcademicPeriod> AcademicPeriods { get; }
    IRepository<Enrollment> Enrollments { get; }
    IRepository<CourseAssignment> CourseAssignments { get; }
    IRepository<Grade> Grades { get; }
    IRepository<Invoice> Invoices { get; }
    IRepository<InvoiceDetail> InvoiceDetails { get; }
    IRepository<Payment> Payments { get; }
    Task<int> SaveChangesAsync();
}
