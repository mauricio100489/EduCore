namespace EduCore.Domain.Entities;

public class Student : BaseEntity
{
    // Datos personales
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;           // cuenta
    public string? IdentificationNumber { get; set; }                    // identidad_alumno
    public string? Gender { get; set; }                                  // sexo
    public DateTime? DateOfBirth { get; set; }                           // fecha_nacimiento
    public string? BirthPlace { get; set; }                              // lugar_nacimiento
    public string? Address { get; set; }                                 // direccion
    public string? Handedness { get; set; }                              // manoescribe
    public byte[]? PhotoData { get; set; }                               // foto_alumno

    // Salud
    public string? MedicalConditions { get; set; }                       // enfermedades
    public bool? HasAllergies { get; set; }                              // padecealergias
    public string? AllergiesDescription { get; set; }                    // cualesalergias

    // Familia / hermanos
    public string? HasSiblings { get; set; }                             // tienehermanos
    public string? SiblingsCount { get; set; }                           // cuantoshermanos
    public string? SiblingsGrades { get; set; }                          // gradoshermanos

    // Observaciones
    public string? ParentObservations { get; set; }                      // obspadre
    public string? ReferralEmails { get; set; }                          // correo_remisiones

    // Computed
    public string FullName => $"{FirstName} {LastName}";

    // Navigation
    public ICollection<StudentGuardian> Guardians { get; set; } = new List<StudentGuardian>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
