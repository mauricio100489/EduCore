namespace EduCore.Domain.Entities;

public class AcademicPeriod : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCurrent { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
