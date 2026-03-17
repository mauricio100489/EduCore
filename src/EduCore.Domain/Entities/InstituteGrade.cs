namespace EduCore.Domain.Entities;

public class InstituteGrade : BaseEntity
{
    public int InstituteId { get; set; }
    public int PeriodId { get; set; }
    public string GradeName { get; set; } = string.Empty; // grado_instituto
    public string? OfficialGrade { get; set; } // grado_oficial
    public string? OfficialModality { get; set; } // modalidad_oficial
    public int Order { get; set; }
    public decimal? EnrollmentFee { get; set; } // matricula
    public decimal? MonthlyFee12 { get; set; } // mensualidad_12
    public decimal? MonthlyFee10 { get; set; } // mensualidad_10
    public decimal? MaterialsFee { get; set; } // materiales_libros
    public decimal? OtherFees { get; set; } // otros
    public string? EvaluationType { get; set; } // evaluacion

    // Email configuration
    public string? EmailSender { get; set; } // CorreoEnvioRemitente
    public string? EmailName { get; set; } // NombreCorreo
    public string? EmailBody { get; set; } // CuerpoCorreo
    public string? SmtpServer { get; set; } // SMTP
    public int? SmtpPort { get; set; } // PuertoSMPT
    public string? EmailPassword { get; set; } // Contraseña
    public string? EmailTitle { get; set; } // TituloCorreo

    // Orientation counselor
    public string? CounselorName { get; set; } // NombreOrientador
    public byte[]? CounselorSignature { get; set; } // FirmaOrientador

    public int? EducationLevelId { get; set; } // idNivelEducativo
    public byte[]? Images { get; set; }

    // Navigation
    public Institute Institute { get; set; } = null!;
    public InstitutePeriod Period { get; set; } = null!;
}
