using EduCore.Domain.Enums;

namespace EduCore.Domain.Entities;

public class Payment : BaseEntity
{
    public int InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public PaymentMethod Method { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Notes { get; set; }

    public Invoice Invoice { get; set; } = null!;
}
