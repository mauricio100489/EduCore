using EduCore.Application.DTOs;
using EduCore.Application.Interfaces;
using EduCore.Domain.Entities;
using EduCore.Domain.Enums;
using EduCore.Domain.Interfaces;

namespace EduCore.Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IUnitOfWork _uow;
    public InvoiceService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<InvoiceDto>> GetAllAsync()
    {
        var items = await _uow.Invoices.GetAllAsync();
        return items.Select(MapToDto);
    }

    public async Task<InvoiceDto?> GetByIdAsync(int id)
    {
        var i = await _uow.Invoices.GetByIdAsync(id);
        return i is null ? null : MapToDto(i);
    }

    public async Task<InvoiceDto> CreateAsync(InvoiceDto dto)
    {
        var entity = new Invoice
        {
            InvoiceNumber = dto.InvoiceNumber, StudentId = dto.StudentId,
            AcademicPeriodId = dto.AcademicPeriodId, IssueDate = dto.IssueDate,
            DueDate = dto.DueDate, Subtotal = dto.Subtotal, Tax = dto.Tax,
            Discount = dto.Discount, Status = dto.Status, Notes = dto.Notes,
            Details = dto.Details.Select(d => new InvoiceDetail
            {
                Concept = d.Concept, Description = d.Description,
                Quantity = d.Quantity, UnitPrice = d.UnitPrice
            }).ToList()
        };
        var created = await _uow.Invoices.AddAsync(entity);
        await _uow.SaveChangesAsync();
        return MapToDto(created);
    }

    public async Task UpdateAsync(InvoiceDto dto)
    {
        var entity = await _uow.Invoices.GetByIdAsync(dto.Id)
            ?? throw new KeyNotFoundException($"Invoice {dto.Id} not found");
        entity.InvoiceNumber = dto.InvoiceNumber; entity.StudentId = dto.StudentId;
        entity.AcademicPeriodId = dto.AcademicPeriodId; entity.DueDate = dto.DueDate;
        entity.Subtotal = dto.Subtotal; entity.Tax = dto.Tax;
        entity.Discount = dto.Discount; entity.Status = dto.Status;
        entity.Notes = dto.Notes; entity.UpdatedAt = DateTime.UtcNow;
        await _uow.Invoices.UpdateAsync(entity);
        await _uow.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _uow.Invoices.DeleteAsync(id);
        await _uow.SaveChangesAsync();
    }

    public async Task<IEnumerable<InvoiceDto>> GetByStudentAsync(int studentId)
    {
        var items = await _uow.Invoices.FindAsync(i => i.StudentId == studentId);
        return items.Select(MapToDto);
    }

    public async Task<PaymentDto> AddPaymentAsync(PaymentDto dto)
    {
        var invoice = await _uow.Invoices.GetByIdAsync(dto.InvoiceId)
            ?? throw new KeyNotFoundException($"Invoice {dto.InvoiceId} not found");

        var payment = new Payment
        {
            InvoiceId = dto.InvoiceId, Amount = dto.Amount,
            PaymentDate = dto.PaymentDate, Method = dto.Method,
            ReferenceNumber = dto.ReferenceNumber, Notes = dto.Notes
        };
        var created = await _uow.Payments.AddAsync(payment);

        var totalPaid = invoice.Payments.Sum(p => p.Amount) + dto.Amount;
        if (totalPaid >= invoice.Total)
            invoice.Status = PaymentStatus.Paid;

        await _uow.SaveChangesAsync();
        return new PaymentDto
        {
            Id = created.Id, InvoiceId = created.InvoiceId,
            Amount = created.Amount, PaymentDate = created.PaymentDate,
            Method = created.Method, ReferenceNumber = created.ReferenceNumber,
            Notes = created.Notes
        };
    }

    private static InvoiceDto MapToDto(Invoice i) => new()
    {
        Id = i.Id, InvoiceNumber = i.InvoiceNumber, StudentId = i.StudentId,
        StudentName = i.Student?.FullName ?? "",
        AcademicPeriodId = i.AcademicPeriodId,
        AcademicPeriodName = i.AcademicPeriod?.Name ?? "",
        IssueDate = i.IssueDate, DueDate = i.DueDate,
        Subtotal = i.Subtotal, Tax = i.Tax, Discount = i.Discount,
        Status = i.Status, Notes = i.Notes,
        Details = i.Details.Select(d => new InvoiceDetailDto
        {
            Id = d.Id, InvoiceId = d.InvoiceId, Concept = d.Concept,
            Description = d.Description, Quantity = d.Quantity, UnitPrice = d.UnitPrice
        }).ToList(),
        Payments = i.Payments.Select(p => new PaymentDto
        {
            Id = p.Id, InvoiceId = p.InvoiceId, Amount = p.Amount,
            PaymentDate = p.PaymentDate, Method = p.Method,
            ReferenceNumber = p.ReferenceNumber, Notes = p.Notes
        }).ToList()
    };
}
