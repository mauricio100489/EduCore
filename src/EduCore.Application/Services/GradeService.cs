using EduCore.Application.DTOs;
using EduCore.Application.Interfaces;
using EduCore.Domain.Entities;
using EduCore.Domain.Interfaces;

namespace EduCore.Application.Services;

public class GradeService : IGradeService
{
    private readonly IUnitOfWork _uow;
    public GradeService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<GradeDto>> GetAllAsync()
    {
        var items = await _uow.Grades.GetAllAsync();
        return items.Select(MapToDto);
    }

    public async Task<GradeDto?> GetByIdAsync(int id)
    {
        var g = await _uow.Grades.GetByIdAsync(id);
        return g is null ? null : MapToDto(g);
    }

    public async Task<GradeDto> CreateAsync(GradeDto dto)
    {
        var entity = new Grade
        {
            StudentId = dto.StudentId, CourseId = dto.CourseId,
            AcademicPeriodId = dto.AcademicPeriodId, Type = dto.Type,
            Description = dto.Description, Score = dto.Score,
            MaxScore = dto.MaxScore, GradeDate = dto.GradeDate,
            Observations = dto.Observations
        };
        var created = await _uow.Grades.AddAsync(entity);
        await _uow.SaveChangesAsync();
        return MapToDto(created);
    }

    public async Task UpdateAsync(GradeDto dto)
    {
        var entity = await _uow.Grades.GetByIdAsync(dto.Id)
            ?? throw new KeyNotFoundException($"Grade {dto.Id} not found");
        entity.StudentId = dto.StudentId; entity.CourseId = dto.CourseId;
        entity.AcademicPeriodId = dto.AcademicPeriodId; entity.Type = dto.Type;
        entity.Description = dto.Description; entity.Score = dto.Score;
        entity.MaxScore = dto.MaxScore; entity.GradeDate = dto.GradeDate;
        entity.Observations = dto.Observations; entity.UpdatedAt = DateTime.UtcNow;
        await _uow.Grades.UpdateAsync(entity);
        await _uow.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _uow.Grades.DeleteAsync(id);
        await _uow.SaveChangesAsync();
    }

    public async Task<IEnumerable<GradeDto>> GetByStudentAsync(int studentId)
    {
        var items = await _uow.Grades.FindAsync(g => g.StudentId == studentId);
        return items.Select(MapToDto);
    }

    public async Task<IEnumerable<GradeDto>> GetByStudentAndCourseAsync(int studentId, int courseId)
    {
        var items = await _uow.Grades.FindAsync(g => g.StudentId == studentId && g.CourseId == courseId);
        return items.Select(MapToDto);
    }

    private static GradeDto MapToDto(Grade g) => new()
    {
        Id = g.Id, StudentId = g.StudentId,
        StudentName = g.Student?.FullName ?? "",
        CourseId = g.CourseId, CourseName = g.Course?.Name ?? "",
        AcademicPeriodId = g.AcademicPeriodId,
        AcademicPeriodName = g.AcademicPeriod?.Name ?? "",
        Type = g.Type, Description = g.Description,
        Score = g.Score, MaxScore = g.MaxScore,
        GradeDate = g.GradeDate, Observations = g.Observations
    };
}
