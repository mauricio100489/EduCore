namespace EduCore.Domain.Entities;

public class InvoiceDetail : BaseEntity
{
    public int InvoiceId { get; set; }
    public string Concept { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public decimal Total => Quantity * UnitPrice;

    public Invoice Invoice { get; set; } = null!;
}
