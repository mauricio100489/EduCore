using EduCore.Application.DTOs;
using EduCore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduCore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ConfigurationController : ControllerBase
{
    private readonly IInstituteService _instituteService;
    private readonly IInstitutePeriodService _periodService;
    private readonly IInstituteGradeService _gradeService;
    private readonly IInstituteShiftService _shiftService;
    private readonly IInstituteSectionService _sectionService;

    public ConfigurationController(
        IInstituteService instituteService,
        IInstitutePeriodService periodService,
        IInstituteGradeService gradeService,
        IInstituteShiftService shiftService,
        IInstituteSectionService sectionService)
    {
        _instituteService = instituteService;
        _periodService = periodService;
        _gradeService = gradeService;
        _shiftService = shiftService;
        _sectionService = sectionService;
    }

    // ── Institutes ──────────────────────────────────────────────────────

    [HttpGet("institutes")]
    public async Task<ActionResult<IEnumerable<InstituteDto>>> GetInstitutes() => Ok(await _instituteService.GetAllAsync());

    [HttpGet("institutes/{id}")]
    public async Task<ActionResult<InstituteDto>> GetInstituteById(int id)
    {
        var item = await _instituteService.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost("institutes")]
    public async Task<ActionResult<InstituteDto>> CreateInstitute([FromBody] InstituteDto dto)
    {
        var created = await _instituteService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetInstituteById), new { id = created.Id }, created);
    }

    [HttpPut("institutes/{id}")]
    public async Task<IActionResult> UpdateInstitute(int id, [FromBody] InstituteDto dto)
    {
        dto.Id = id;
        await _instituteService.UpdateAsync(dto);
        return NoContent();
    }

    [HttpDelete("institutes/{id}")]
    public async Task<IActionResult> DeleteInstitute(int id)
    {
        await _instituteService.DeleteAsync(id);
        return NoContent();
    }

    // ── Periods ─────────────────────────────────────────────────────────

    [HttpGet("periods")]
    public async Task<ActionResult<IEnumerable<InstitutePeriodDto>>> GetPeriods() => Ok(await _periodService.GetAllAsync());

    [HttpGet("periods/by-institute/{instituteId}")]
    public async Task<ActionResult<IEnumerable<InstitutePeriodDto>>> GetPeriodsByInstitute(int instituteId) =>
        Ok(await _periodService.GetByInstituteAsync(instituteId));

    [HttpGet("periods/{id}")]
    public async Task<ActionResult<InstitutePeriodDto>> GetPeriodById(int id)
    {
        var item = await _periodService.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost("periods")]
    public async Task<ActionResult<InstitutePeriodDto>> CreatePeriod([FromBody] InstitutePeriodDto dto)
    {
        var created = await _periodService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetPeriodById), new { id = created.Id }, created);
    }

    [HttpPut("periods/{id}")]
    public async Task<IActionResult> UpdatePeriod(int id, [FromBody] InstitutePeriodDto dto)
    {
        dto.Id = id;
        await _periodService.UpdateAsync(dto);
        return NoContent();
    }

    [HttpDelete("periods/{id}")]
    public async Task<IActionResult> DeletePeriod(int id)
    {
        await _periodService.DeleteAsync(id);
        return NoContent();
    }

    // ── Grades ──────────────────────────────────────────────────────────

    [HttpGet("grades")]
    public async Task<ActionResult<IEnumerable<InstituteGradeDto>>> GetGrades() => Ok(await _gradeService.GetAllAsync());

    [HttpGet("grades/by-institute-period/{instituteId}/{periodId}")]
    public async Task<ActionResult<IEnumerable<InstituteGradeDto>>> GetGradesByInstitutePeriod(int instituteId, int periodId) =>
        Ok(await _gradeService.GetByInstituteAndPeriodAsync(instituteId, periodId));

    [HttpGet("grades/{id}")]
    public async Task<ActionResult<InstituteGradeDto>> GetGradeById(int id)
    {
        var item = await _gradeService.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost("grades")]
    public async Task<ActionResult<InstituteGradeDto>> CreateGrade([FromBody] InstituteGradeDto dto)
    {
        var created = await _gradeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetGradeById), new { id = created.Id }, created);
    }

    [HttpPut("grades/{id}")]
    public async Task<IActionResult> UpdateGrade(int id, [FromBody] InstituteGradeDto dto)
    {
        dto.Id = id;
        await _gradeService.UpdateAsync(dto);
        return NoContent();
    }

    [HttpDelete("grades/{id}")]
    public async Task<IActionResult> DeleteGrade(int id)
    {
        await _gradeService.DeleteAsync(id);
        return NoContent();
    }

    // ── Shifts ──────────────────────────────────────────────────────────

    [HttpGet("shifts")]
    public async Task<ActionResult<IEnumerable<InstituteShiftDto>>> GetShifts() => Ok(await _shiftService.GetAllAsync());

    [HttpGet("shifts/by-institute-period/{instituteId}/{periodId}")]
    public async Task<ActionResult<IEnumerable<InstituteShiftDto>>> GetShiftsByInstitutePeriod(int instituteId, int periodId) =>
        Ok(await _shiftService.GetByInstituteAndPeriodAsync(instituteId, periodId));

    [HttpGet("shifts/{id}")]
    public async Task<ActionResult<InstituteShiftDto>> GetShiftById(int id)
    {
        var item = await _shiftService.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost("shifts")]
    public async Task<ActionResult<InstituteShiftDto>> CreateShift([FromBody] InstituteShiftDto dto)
    {
        var created = await _shiftService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetShiftById), new { id = created.Id }, created);
    }

    [HttpPut("shifts/{id}")]
    public async Task<IActionResult> UpdateShift(int id, [FromBody] InstituteShiftDto dto)
    {
        dto.Id = id;
        await _shiftService.UpdateAsync(dto);
        return NoContent();
    }

    [HttpDelete("shifts/{id}")]
    public async Task<IActionResult> DeleteShift(int id)
    {
        await _shiftService.DeleteAsync(id);
        return NoContent();
    }

    // ── Sections ────────────────────────────────────────────────────────

    [HttpGet("sections")]
    public async Task<ActionResult<IEnumerable<InstituteSectionDto>>> GetSections() => Ok(await _sectionService.GetAllAsync());

    [HttpGet("sections/by-institute-period/{instituteId}/{periodId}")]
    public async Task<ActionResult<IEnumerable<InstituteSectionDto>>> GetSectionsByInstitutePeriod(int instituteId, int periodId) =>
        Ok(await _sectionService.GetByInstituteAndPeriodAsync(instituteId, periodId));

    [HttpGet("sections/{id}")]
    public async Task<ActionResult<InstituteSectionDto>> GetSectionById(int id)
    {
        var item = await _sectionService.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost("sections")]
    public async Task<ActionResult<InstituteSectionDto>> CreateSection([FromBody] InstituteSectionDto dto)
    {
        var created = await _sectionService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetSectionById), new { id = created.Id }, created);
    }

    [HttpPut("sections/{id}")]
    public async Task<IActionResult> UpdateSection(int id, [FromBody] InstituteSectionDto dto)
    {
        dto.Id = id;
        await _sectionService.UpdateAsync(dto);
        return NoContent();
    }

    [HttpDelete("sections/{id}")]
    public async Task<IActionResult> DeleteSection(int id)
    {
        await _sectionService.DeleteAsync(id);
        return NoContent();
    }
}
