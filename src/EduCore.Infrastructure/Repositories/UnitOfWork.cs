using EduCore.Domain.Entities;
using EduCore.Domain.Interfaces;
using EduCore.Infrastructure.Data;

namespace EduCore.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly EduCoreDbContext _context;

    public UnitOfWork(EduCoreDbContext context)
    {
        _context = context;
        Students = new Repository<Student>(context);
        StudentGuardians = new Repository<StudentGuardian>(context);
        Teachers = new Repository<Teacher>(context);
        Courses = new Repository<Course>(context);
        GradeLevels = new Repository<GradeLevel>(context);
        AcademicPeriods = new Repository<AcademicPeriod>(context);
        Enrollments = new Repository<Enrollment>(context);
        CourseAssignments = new Repository<CourseAssignment>(context);
        Grades = new Repository<Grade>(context);
        Invoices = new Repository<Invoice>(context);
        InvoiceDetails = new Repository<InvoiceDetail>(context);
        Payments = new Repository<Payment>(context);
    }

    public IRepository<Student> Students { get; }
    public IRepository<StudentGuardian> StudentGuardians { get; }
    public IRepository<Teacher> Teachers { get; }
    public IRepository<Course> Courses { get; }
    public IRepository<GradeLevel> GradeLevels { get; }
    public IRepository<AcademicPeriod> AcademicPeriods { get; }
    public IRepository<Enrollment> Enrollments { get; }
    public IRepository<CourseAssignment> CourseAssignments { get; }
    public IRepository<Grade> Grades { get; }
    public IRepository<Invoice> Invoices { get; }
    public IRepository<InvoiceDetail> InvoiceDetails { get; }
    public IRepository<Payment> Payments { get; }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}
