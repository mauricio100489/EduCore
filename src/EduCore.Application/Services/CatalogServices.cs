using EduCore.Application.DTOs;
using EduCore.Application.Interfaces;
using EduCore.Domain.Entities;
using EduCore.Domain.Interfaces;

namespace EduCore.Application.Services;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork _uow;
    public CourseService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<CourseDto>> GetAllAsync()
    {
        var items = await _uow.Courses.GetAllAsync();
        return items.Select(c => new CourseDto
        {
            Id = c.Id, Code = c.Code, Name = c.Name, Description = c.Description,
            Credits = c.Credits, HoursPerWeek = c.HoursPerWeek,
            GradeLevelId = c.GradeLevelId,
            GradeLevelName = c.GradeLevel?.Name ?? "", IsActive = c.IsActive
        });
    }

    public async Task<CourseDto?> GetByIdAsync(int id)
    {
        var c = await _uow.Courses.GetByIdAsync(id);
        if (c is null) return null;
        return new CourseDto
        {
            Id = c.Id, Code = c.Code, Name = c.Name, Description = c.Description,
            Credits = c.Credits, HoursPerWeek = c.HoursPerWeek,
            GradeLevelId = c.GradeLevelId,
            GradeLevelName = c.GradeLevel?.Name ?? "", IsActive = c.IsActive
        };
    }

    public async Task<CourseDto> CreateAsync(CourseDto dto)
    {
        var entity = new Course
        {
            Code = dto.Code, Name = dto.Name, Description = dto.Description,
            Credits = dto.Credits, HoursPerWeek = dto.HoursPerWeek,
            GradeLevelId = dto.GradeLevelId
        };
        var created = await _uow.Courses.AddAsync(entity);
        await _uow.SaveChangesAsync();
        return new CourseDto { Id = created.Id, Code = created.Code, Name = created.Name,
            Description = created.Description, Credits = created.Credits,
            HoursPerWeek = created.HoursPerWeek, GradeLevelId = created.GradeLevelId,
            IsActive = created.IsActive };
    }

    public async Task UpdateAsync(CourseDto dto)
    {
        var entity = await _uow.Courses.GetByIdAsync(dto.Id)
            ?? throw new KeyNotFoundException($"Course {dto.Id} not found");
        entity.Code = dto.Code; entity.Name = dto.Name; entity.Description = dto.Description;
        entity.Credits = dto.Credits; entity.HoursPerWeek = dto.HoursPerWeek;
        entity.GradeLevelId = dto.GradeLevelId; entity.IsActive = dto.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _uow.Courses.UpdateAsync(entity);
        await _uow.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _uow.Courses.DeleteAsync(id);
        await _uow.SaveChangesAsync();
    }
}

public class GradeLevelService : IGradeLevelService
{
    private readonly IUnitOfWork _uow;
    public GradeLevelService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<GradeLevelDto>> GetAllAsync()
    {
        var items = await _uow.GradeLevels.GetAllAsync();
        return items.Select(g => new GradeLevelDto
        {
            Id = g.Id, Name = g.Name, Description = g.Description,
            Order = g.Order, Section = g.Section, IsActive = g.IsActive
        });
    }

    public async Task<GradeLevelDto?> GetByIdAsync(int id)
    {
        var g = await _uow.GradeLevels.GetByIdAsync(id);
        if (g is null) return null;
        return new GradeLevelDto { Id = g.Id, Name = g.Name, Description = g.Description,
            Order = g.Order, Section = g.Section, IsActive = g.IsActive };
    }

    public async Task<GradeLevelDto> CreateAsync(GradeLevelDto dto)
    {
        var entity = new GradeLevel { Name = dto.Name, Description = dto.Description,
            Order = dto.Order, Section = dto.Section };
        var created = await _uow.GradeLevels.AddAsync(entity);
        await _uow.SaveChangesAsync();
        return new GradeLevelDto { Id = created.Id, Name = created.Name,
            Description = created.Description, Order = created.Order,
            Section = created.Section, IsActive = created.IsActive };
    }

    public async Task UpdateAsync(GradeLevelDto dto)
    {
        var entity = await _uow.GradeLevels.GetByIdAsync(dto.Id)
            ?? throw new KeyNotFoundException($"GradeLevel {dto.Id} not found");
        entity.Name = dto.Name; entity.Description = dto.Description;
        entity.Order = dto.Order; entity.Section = dto.Section;
        entity.IsActive = dto.IsActive; entity.UpdatedAt = DateTime.UtcNow;
        await _uow.GradeLevels.UpdateAsync(entity);
        await _uow.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _uow.GradeLevels.DeleteAsync(id);
        await _uow.SaveChangesAsync();
    }
}

public class AcademicPeriodService : IAcademicPeriodService
{
    private readonly IUnitOfWork _uow;
    public AcademicPeriodService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<AcademicPeriodDto>> GetAllAsync()
    {
        var items = await _uow.AcademicPeriods.GetAllAsync();
        return items.Select(a => new AcademicPeriodDto
        {
            Id = a.Id, Name = a.Name, Year = a.Year, StartDate = a.StartDate,
            EndDate = a.EndDate, IsCurrent = a.IsCurrent, IsActive = a.IsActive
        });
    }

    public async Task<AcademicPeriodDto?> GetByIdAsync(int id)
    {
        var a = await _uow.AcademicPeriods.GetByIdAsync(id);
        if (a is null) return null;
        return new AcademicPeriodDto { Id = a.Id, Name = a.Name, Year = a.Year,
            StartDate = a.StartDate, EndDate = a.EndDate, IsCurrent = a.IsCurrent,
            IsActive = a.IsActive };
    }

    public async Task<AcademicPeriodDto> CreateAsync(AcademicPeriodDto dto)
    {
        var entity = new AcademicPeriod { Name = dto.Name, Year = dto.Year,
            StartDate = dto.StartDate, EndDate = dto.EndDate, IsCurrent = dto.IsCurrent };
        var created = await _uow.AcademicPeriods.AddAsync(entity);
        await _uow.SaveChangesAsync();
        return new AcademicPeriodDto { Id = created.Id, Name = created.Name,
            Year = created.Year, StartDate = created.StartDate,
            EndDate = created.EndDate, IsCurrent = created.IsCurrent,
            IsActive = created.IsActive };
    }

    public async Task UpdateAsync(AcademicPeriodDto dto)
    {
        var entity = await _uow.AcademicPeriods.GetByIdAsync(dto.Id)
            ?? throw new KeyNotFoundException($"AcademicPeriod {dto.Id} not found");
        entity.Name = dto.Name; entity.Year = dto.Year;
        entity.StartDate = dto.StartDate; entity.EndDate = dto.EndDate;
        entity.IsCurrent = dto.IsCurrent; entity.IsActive = dto.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _uow.AcademicPeriods.UpdateAsync(entity);
        await _uow.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _uow.AcademicPeriods.DeleteAsync(id);
        await _uow.SaveChangesAsync();
    }
}
