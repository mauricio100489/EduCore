using EduCore.Application.DTOs;
using EduCore.Application.Interfaces;
using EduCore.Domain.Enums;
using EduCore.Domain.Interfaces;

namespace EduCore.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _uow;
    public DashboardService(IUnitOfWork uow) => _uow = uow;

    public async Task<DashboardDto> GetDashboardAsync()
    {
        var students = await _uow.Students.CountAsync(s => s.IsActive);
        var teachers = await _uow.Teachers.CountAsync(t => t.IsActive);
        var courses = await _uow.Courses.CountAsync(c => c.IsActive);
        var enrollments = await _uow.Enrollments.CountAsync(e => e.Status == EnrollmentStatus.Active);

        var invoices = await _uow.Invoices.GetAllAsync();
        var paidInvoices = invoices.Where(i => i.Status == PaymentStatus.Paid);
        var pendingInvoices = invoices.Where(i => i.Status == PaymentStatus.Pending);

        var gradeLevels = await _uow.GradeLevels.GetAllAsync();
        var allEnrollments = await _uow.Enrollments.FindAsync(e => e.Status == EnrollmentStatus.Active);

        return new DashboardDto
        {
            TotalStudents = students,
            ActiveEnrollments = enrollments,
            TotalTeachers = teachers,
            TotalCourses = courses,
            TotalRevenue = paidInvoices.Sum(i => i.Total),
            PendingPayments = pendingInvoices.Sum(i => i.Total),
            MonthlyRevenue = paidInvoices
                .GroupBy(i => i.IssueDate.ToString("MMM yyyy"))
                .Select(g => new MonthlyRevenueDto { Month = g.Key, Amount = g.Sum(i => i.Total) })
                .OrderBy(m => m.Month)
                .Take(12)
                .ToList(),
            EnrollmentsByGrade = gradeLevels.Select(gl => new EnrollmentsByGradeDto
            {
                GradeLevel = gl.Name,
                Count = allEnrollments.Count(e => e.GradeLevelId == gl.Id)
            }).ToList()
        };
    }
}
