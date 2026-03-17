using EduCore.Domain.Enums;

namespace EduCore.Domain.Entities;

public class Enrollment : BaseEntity
{
    // Relaciones existentes
    public int StudentId { get; set; }
    public int GradeLevelId { get; set; }
    public int AcademicPeriodId { get; set; }

    // Datos academicos de matricula
    public string? Section { get; set; }                             // seccion
    public string? Shift { get; set; }                               // jornada
    public string? ListNumber { get; set; }                          // no_lista
    public DateTime? EnrollmentDate { get; set; }                    // fecha_matricula
    public string? EnrollmentMethod { get; set; }                    // FormaMatricula
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Pending;
    public string? Notes { get; set; }

    // Instituto de procedencia
    public string? PreviousInstitute { get; set; }                   // instituto_procede
    public string? PreviousInstituteAddress { get; set; }            // dir_instituto_procede
    public string? PreviousGrade { get; set; }                       // grado_procede
    public string? PreviousShift { get; set; }                       // jornada_procede

    // Becas y descuentos
    public bool? HasScholarship { get; set; }                        // beca
    public decimal? ScholarshipAmount { get; set; }                  // valor_beca
    public bool? FullScholarship { get; set; }                       // beca_completa
    public bool? HasEnrollmentDiscount { get; set; }                 // descu_matri
    public decimal? DiscountAmount { get; set; }                     // valor_descu
    public decimal? LevelingFee { get; set; }                        // cnivelada
    public string? PaymentForm { get; set; }                         // forma_pago

    // Flags de estado
    public bool? IsWithdrawn { get; set; }                           // retirado
    public bool? IsAuditor { get; set; }                             // oyente
    public bool? IsTransfer { get; set; }                            // traslado
    public bool? IsReentry { get; set; }                             // reingreso
    public bool? AcceptedTerms { get; set; }                         // aceptoterminos
    public bool? ReportCardPrinted { get; set; }                     // libretaImpresa

    // Firma y observaciones
    public byte[]? SignatureData { get; set; }                       // firma
    public string? GradeObservations { get; set; }                   // ObsCalificaciones

    // Referencia sistema legado
    public int? LegacyEnrollmentId { get; set; }                     // id_matricula

    // Navigation
    public Student Student { get; set; } = null!;
    public GradeLevel GradeLevel { get; set; } = null!;
    public AcademicPeriod AcademicPeriod { get; set; } = null!;
}
