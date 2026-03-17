using EduCore.Domain.Entities;
using EduCore.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduCore.Infrastructure.Data;

public class EduCoreDbContext : IdentityDbContext<ApplicationUser>
{
    public EduCoreDbContext(DbContextOptions<EduCoreDbContext> options) : base(options) { }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<StudentGuardian> StudentGuardians => Set<StudentGuardian>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<GradeLevel> GradeLevels => Set<GradeLevel>();
    public DbSet<AcademicPeriod> AcademicPeriods => Set<AcademicPeriod>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<CourseAssignment> CourseAssignments => Set<CourseAssignment>();
    public DbSet<Grade> Grades => Set<Grade>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceDetail> InvoiceDetails => Set<InvoiceDetail>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Institute> Institutes => Set<Institute>();
    public DbSet<InstitutePeriod> InstitutePeriods => Set<InstitutePeriod>();
    public DbSet<InstituteGrade> InstituteGrades => Set<InstituteGrade>();
    public DbSet<InstituteShift> InstituteShifts => Set<InstituteShift>();
    public DbSet<InstituteSection> InstituteSections => Set<InstituteSection>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ===== Student =====
        builder.Entity<Student>(e =>
        {
            e.HasIndex(s => s.AccountNumber).IsUnique();
            e.HasIndex(s => s.IdentificationNumber);
            e.Property(s => s.FirstName).HasMaxLength(100);
            e.Property(s => s.LastName).HasMaxLength(100);
            e.Property(s => s.AccountNumber).HasMaxLength(6);
            e.Property(s => s.IdentificationNumber).HasMaxLength(15);
            e.Property(s => s.Gender).HasMaxLength(15);
            e.Property(s => s.BirthPlace).HasMaxLength(150);
            e.Property(s => s.Address).HasMaxLength(200);
            e.Property(s => s.Handedness).HasMaxLength(50);
            e.Property(s => s.MedicalConditions).HasMaxLength(1000);
            e.Property(s => s.AllergiesDescription).HasMaxLength(1000);
            e.Property(s => s.HasSiblings).HasMaxLength(2);
            e.Property(s => s.SiblingsCount).HasMaxLength(50);
            e.Property(s => s.SiblingsGrades).HasMaxLength(1000);
            e.Property(s => s.ParentObservations).HasMaxLength(3000);
            e.Property(s => s.ReferralEmails).HasMaxLength(3000);
        });

        // ===== StudentGuardian =====
        builder.Entity<StudentGuardian>(e =>
        {
            e.HasOne(g => g.Student).WithMany(s => s.Guardians)
                .HasForeignKey(g => g.StudentId).OnDelete(DeleteBehavior.Cascade);
            e.Property(g => g.IdentificationNumber).HasMaxLength(15);
            e.Property(g => g.FullName).HasMaxLength(150);
            e.Property(g => g.Phones).HasMaxLength(150);
            e.Property(g => g.Email).HasMaxLength(150);
        });

        // ===== Teacher =====
        builder.Entity<Teacher>(e =>
        {
            e.HasIndex(t => t.IdentificationNumber).IsUnique();
            e.Property(t => t.FirstName).HasMaxLength(100);
            e.Property(t => t.LastName).HasMaxLength(100);
        });

        // ===== Course =====
        builder.Entity<Course>(e =>
        {
            e.HasIndex(c => c.Code).IsUnique();
            e.Property(c => c.Name).HasMaxLength(200);
            e.HasOne(c => c.GradeLevel).WithMany(gl => gl.Courses)
                .HasForeignKey(c => c.GradeLevelId);
        });

        builder.Entity<GradeLevel>(e =>
        {
            e.Property(g => g.Name).HasMaxLength(100);
        });

        builder.Entity<AcademicPeriod>(e =>
        {
            e.Property(a => a.Name).HasMaxLength(100);
        });

        // ===== Enrollment =====
        builder.Entity<Enrollment>(e =>
        {
            e.HasOne(en => en.Student).WithMany(s => s.Enrollments)
                .HasForeignKey(en => en.StudentId);
            e.HasOne(en => en.GradeLevel).WithMany(gl => gl.Enrollments)
                .HasForeignKey(en => en.GradeLevelId);
            e.HasOne(en => en.AcademicPeriod).WithMany(ap => ap.Enrollments)
                .HasForeignKey(en => en.AcademicPeriodId);
            e.Property(en => en.Section).HasMaxLength(15);
            e.Property(en => en.Shift).HasMaxLength(20);
            e.Property(en => en.ListNumber).HasMaxLength(3);
            e.Property(en => en.EnrollmentMethod).HasMaxLength(50);
            e.Property(en => en.PreviousInstitute).HasMaxLength(150);
            e.Property(en => en.PreviousInstituteAddress).HasMaxLength(150);
            e.Property(en => en.PreviousGrade).HasMaxLength(150);
            e.Property(en => en.PreviousShift).HasMaxLength(20);
            e.Property(en => en.PaymentForm).HasMaxLength(2);
            e.Property(en => en.GradeObservations).HasMaxLength(1000);
            e.Property(en => en.ScholarshipAmount).HasPrecision(18, 2);
            e.Property(en => en.DiscountAmount).HasPrecision(18, 2);
            e.Property(en => en.LevelingFee).HasPrecision(18, 2);
        });

        // ===== CourseAssignment =====
        builder.Entity<CourseAssignment>(e =>
        {
            e.HasOne(ca => ca.Teacher).WithMany(t => t.CourseAssignments)
                .HasForeignKey(ca => ca.TeacherId);
            e.HasOne(ca => ca.Course).WithMany(c => c.CourseAssignments)
                .HasForeignKey(ca => ca.CourseId);
        });

        // ===== Grade =====
        builder.Entity<Grade>(e =>
        {
            e.HasOne(g => g.Student).WithMany(s => s.Grades)
                .HasForeignKey(g => g.StudentId);
            e.HasOne(g => g.Course).WithMany(c => c.Grades)
                .HasForeignKey(g => g.CourseId);
            e.HasOne(g => g.AcademicPeriod).WithMany(ap => ap.Grades)
                .HasForeignKey(g => g.AcademicPeriodId);
            e.Property(g => g.Score).HasPrecision(5, 2);
            e.Property(g => g.MaxScore).HasPrecision(5, 2);
        });

        // ===== Invoice =====
        builder.Entity<Invoice>(e =>
        {
            e.HasIndex(i => i.InvoiceNumber).IsUnique();
            e.HasOne(i => i.Student).WithMany(s => s.Invoices)
                .HasForeignKey(i => i.StudentId);
            e.HasOne(i => i.AcademicPeriod).WithMany(ap => ap.Invoices)
                .HasForeignKey(i => i.AcademicPeriodId);
            e.Property(i => i.Subtotal).HasPrecision(18, 2);
            e.Property(i => i.Tax).HasPrecision(18, 2);
            e.Property(i => i.Discount).HasPrecision(18, 2);
        });

        builder.Entity<InvoiceDetail>(e =>
        {
            e.HasOne(d => d.Invoice).WithMany(i => i.Details)
                .HasForeignKey(d => d.InvoiceId);
            e.Property(d => d.UnitPrice).HasPrecision(18, 2);
        });

        builder.Entity<Payment>(e =>
        {
            e.HasOne(p => p.Invoice).WithMany(i => i.Payments)
                .HasForeignKey(p => p.InvoiceId);
            e.Property(p => p.Amount).HasPrecision(18, 2);
        });

        // ===== Institute =====
        builder.Entity<Institute>(e =>
        {
            e.HasIndex(i => i.Code).IsUnique();
            e.Property(i => i.Code).HasMaxLength(25);
            e.Property(i => i.Rtn).HasMaxLength(50);
            e.Property(i => i.Name).HasMaxLength(150);
            e.Property(i => i.Slogan).HasMaxLength(150);
            e.Property(i => i.Address).HasMaxLength(200);
            e.Property(i => i.Phones).HasMaxLength(100);
            e.Property(i => i.Director).HasMaxLength(100);
            e.Property(i => i.Secretary).HasMaxLength(100);
        });

        // ===== InstitutePeriod =====
        builder.Entity<InstitutePeriod>(e =>
        {
            e.Property(p => p.PeriodName).HasMaxLength(9);
            e.Property(p => p.PromotionName).HasMaxLength(150);
            e.HasOne(p => p.Institute).WithMany(i => i.Periods)
                .HasForeignKey(p => p.InstituteId).OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(p => new { p.InstituteId, p.PeriodName }).IsUnique();
        });

        // ===== InstituteGrade =====
        builder.Entity<InstituteGrade>(e =>
        {
            e.Property(g => g.GradeName).HasMaxLength(150);
            e.Property(g => g.OfficialGrade).HasMaxLength(150);
            e.Property(g => g.OfficialModality).HasMaxLength(150);
            e.Property(g => g.EvaluationType).HasMaxLength(50);
            e.Property(g => g.EmailSender).HasMaxLength(50);
            e.Property(g => g.EmailName).HasMaxLength(50);
            e.Property(g => g.EmailBody).HasMaxLength(3000);
            e.Property(g => g.SmtpServer).HasMaxLength(50);
            e.Property(g => g.EmailPassword).HasMaxLength(50);
            e.Property(g => g.EmailTitle).HasMaxLength(50);
            e.Property(g => g.CounselorName).HasMaxLength(50);
            e.Property(g => g.EnrollmentFee).HasPrecision(18, 2);
            e.Property(g => g.MonthlyFee12).HasPrecision(18, 2);
            e.Property(g => g.MonthlyFee10).HasPrecision(18, 2);
            e.Property(g => g.MaterialsFee).HasPrecision(18, 2);
            e.Property(g => g.OtherFees).HasPrecision(18, 2);
            e.HasOne(g => g.Institute).WithMany(i => i.Grades)
                .HasForeignKey(g => g.InstituteId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(g => g.Period).WithMany(p => p.Grades)
                .HasForeignKey(g => g.PeriodId).OnDelete(DeleteBehavior.Restrict);
        });

        // ===== InstituteShift =====
        builder.Entity<InstituteShift>(e =>
        {
            e.Property(s => s.ShiftName).HasMaxLength(20);
            e.HasOne(s => s.Institute).WithMany(i => i.Shifts)
                .HasForeignKey(s => s.InstituteId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(s => s.Period).WithMany(p => p.Shifts)
                .HasForeignKey(s => s.PeriodId).OnDelete(DeleteBehavior.Restrict);
        });

        // ===== InstituteSection =====
        builder.Entity<InstituteSection>(e =>
        {
            e.Property(s => s.SectionName).HasMaxLength(15);
            e.HasOne(s => s.Institute).WithMany(i => i.Sections)
                .HasForeignKey(s => s.InstituteId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(s => s.Period).WithMany(p => p.Sections)
                .HasForeignKey(s => s.PeriodId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
