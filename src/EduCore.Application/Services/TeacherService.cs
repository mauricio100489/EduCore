using EduCore.Application.DTOs;
using EduCore.Application.Interfaces;
using EduCore.Domain.Entities;
using EduCore.Domain.Interfaces;

namespace EduCore.Application.Services;

public class TeacherService : ITeacherService
{
    private readonly IUnitOfWork _unitOfWork;

    public TeacherService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<IEnumerable<TeacherDto>> GetAllAsync()
    {
        var teachers = await _unitOfWork.Teachers.GetAllAsync();
        return teachers.Select(MapToDto);
    }

    public async Task<TeacherDto?> GetByIdAsync(int id)
    {
        var teacher = await _unitOfWork.Teachers.GetByIdAsync(id);
        return teacher is null ? null : MapToDto(teacher);
    }

    public async Task<TeacherDto> CreateAsync(TeacherDto dto)
    {
        var entity = new Teacher
        {
            FirstName = dto.FirstName, LastName = dto.LastName,
            IdentificationNumber = dto.IdentificationNumber, Email = dto.Email,
            Phone = dto.Phone, Specialty = dto.Specialty, Degree = dto.Degree,
            HireDate = dto.HireDate, PhotoUrl = dto.PhotoUrl
        };
        var created = await _unitOfWork.Teachers.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(created);
    }

    public async Task UpdateAsync(TeacherDto dto)
    {
        var entity = await _unitOfWork.Teachers.GetByIdAsync(dto.Id)
            ?? throw new KeyNotFoundException($"Teacher {dto.Id} not found");
        entity.FirstName = dto.FirstName; entity.LastName = dto.LastName;
        entity.IdentificationNumber = dto.IdentificationNumber; entity.Email = dto.Email;
        entity.Phone = dto.Phone; entity.Specialty = dto.Specialty;
        entity.Degree = dto.Degree; entity.HireDate = dto.HireDate;
        entity.PhotoUrl = dto.PhotoUrl; entity.IsActive = dto.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.Teachers.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _unitOfWork.Teachers.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    private static TeacherDto MapToDto(Teacher t) => new()
    {
        Id = t.Id, FirstName = t.FirstName, LastName = t.LastName,
        IdentificationNumber = t.IdentificationNumber, Email = t.Email,
        Phone = t.Phone, Specialty = t.Specialty, Degree = t.Degree,
        HireDate = t.HireDate, PhotoUrl = t.PhotoUrl, IsActive = t.IsActive
    };
}
