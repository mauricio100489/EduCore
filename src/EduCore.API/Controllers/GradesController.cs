using EduCore.Application.DTOs;
using EduCore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduCore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GradesController : ControllerBase
{
    private readonly IGradeService _service;
    public GradesController(IGradeService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GradeDto>>> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<GradeDto>> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<GradeDto>> Create([FromBody] GradeDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] GradeDto dto)
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
    public async Task<ActionResult<IEnumerable<GradeDto>>> GetByStudent(int studentId) => Ok(await _service.GetByStudentAsync(studentId));

    [HttpGet("student/{studentId}/course/{courseId}")]
    public async Task<ActionResult<IEnumerable<GradeDto>>> GetByStudentAndCourse(int studentId, int courseId) => Ok(await _service.GetByStudentAndCourseAsync(studentId, courseId));
}
