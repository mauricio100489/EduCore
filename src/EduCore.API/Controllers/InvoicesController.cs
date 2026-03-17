using EduCore.Application.DTOs;
using EduCore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduCore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceService _service;
    public InvoicesController(IInvoiceService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<InvoiceDto>> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<InvoiceDto>> Create([FromBody] InvoiceDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] InvoiceDto dto)
    {
        dto.Id = id;
        await _service.UpdateAsync(dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("student/{studentId}")]
    public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetByStudent(int studentId) => Ok(await _service.GetByStudentAsync(studentId));

    [HttpPost("payment")]
    public async Task<ActionResult<PaymentDto>> AddPayment([FromBody] PaymentDto dto)
    {
        var created = await _service.AddPaymentAsync(dto);
        return Ok(created);
    }
}
