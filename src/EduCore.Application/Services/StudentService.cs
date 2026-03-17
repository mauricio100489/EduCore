using EduCore.Application.DTOs;
using EduCore.Application.Interfaces;
using EduCore.Domain.Entities;
using EduCore.Domain.Interfaces;

namespace EduCore.Application.Services;

public class StudentService : IStudentService
{
    private readonly IUnitOfWork _unitOfWork;

    public StudentService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<IEnumerable<StudentDto>> GetAllAsync()
    {
        var students = await _unitOfWork.Students.GetAllAsync();
        var result = new List<StudentDto>();
        foreach (var s in students)
        {
            s.Guardians = (await _unitOfWork.StudentGuardians
                .FindAsync(g => g.StudentId == s.Id)).ToList();
            result.Add(MapToDto(s));
        }
        return result;
    }

    public async Task<StudentDto?> GetByIdAsync(int id)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(id);
        if (student is null) return null;
        student.Guardians = (await _unitOfWork.StudentGuardians
            .FindAsync(g => g.StudentId == student.Id)).ToList();
        return MapToDto(student);
    }

    public async Task<StudentDto> CreateAsync(StudentDto dto)
    {
        var entity = MapToEntity(dto);
        entity.Guardians = dto.Guardians.Select(g => new StudentGuardian
        {
            Type = g.Type,
            IdentificationNumber = g.IdentificationNumber,
            FullName = g.FullName,
            Phones = g.Phones,
            Email = g.Email
        }).ToList();

        var created = await _unitOfWork.Students.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(created);
    }

    public async Task UpdateAsync(StudentDto dto)
    {
        var entity = await _unitOfWork.Students.GetByIdAsync(dto.Id)
            ?? throw new KeyNotFoundException($"Student {dto.Id} not found");

        entity.FirstName = dto.FirstName;
        entity.LastName = dto.LastName;
        entity.AccountNumber = dto.AccountNumber;
        entity.IdentificationNumber = dto.IdentificationNumber;
        entity.Gender = dto.Gender;
        entity.DateOfBirth = dto.DateOfBirth;
        entity.BirthPlace = dto.BirthPlace;
        entity.Address = dto.Address;
        entity.Handedness = dto.Handedness;
        entity.MedicalConditions = dto.MedicalConditions;
        entity.HasAllergies = dto.HasAllergies;
        entity.AllergiesDescription = dto.AllergiesDescription;
        entity.HasSiblings = dto.HasSiblings;
        entity.SiblingsCount = dto.SiblingsCount;
        entity.SiblingsGrades = dto.SiblingsGrades;
        entity.ParentObservations = dto.ParentObservations;
        entity.ReferralEmails = dto.ReferralEmails;
        entity.IsActive = dto.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrEmpty(dto.PhotoBase64))
            entity.PhotoData = Convert.FromBase64String(dto.PhotoBase64);

        // Sync guardians: remove existing and re-add
        var existingGuardians = await _unitOfWork.StudentGuardians
            .FindAsync(g => g.StudentId == dto.Id);
        foreach (var g in existingGuardians)
            await _unitOfWork.StudentGuardians.DeleteAsync(g.Id);

        foreach (var g in dto.Guardians)
        {
            await _unitOfWork.StudentGuardians.AddAsync(new StudentGuardian
            {
                StudentId = dto.Id,
                Type = g.Type,
                IdentificationNumber = g.IdentificationNumber,
                FullName = g.FullName,
                Phones = g.Phones,
                Email = g.Email
            });
        }

        await _unitOfWork.Students.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _unitOfWork.Students.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<StudentDto>> SearchAsync(string term)
    {
        var students = await _unitOfWork.Students.FindAsync(s =>
            s.FirstName.Contains(term) || s.LastName.Contains(term) ||
            s.AccountNumber.Contains(term) ||
            (s.IdentificationNumber != null && s.IdentificationNumber.Contains(term)));
        return students.Select(MapToDto);
    }

    private static StudentDto MapToDto(Student s) => new()
    {
        Id = s.Id,
        FirstName = s.FirstName,
        LastName = s.LastName,
        AccountNumber = s.AccountNumber,
        IdentificationNumber = s.IdentificationNumber,
        Gender = s.Gender,
        DateOfBirth = s.DateOfBirth,
        BirthPlace = s.BirthPlace,
        Address = s.Address,
        Handedness = s.Handedness,
        PhotoBase64 = s.PhotoData != null ? Convert.ToBase64String(s.PhotoData) : null,
        MedicalConditions = s.MedicalConditions,
        HasAllergies = s.HasAllergies,
        AllergiesDescription = s.AllergiesDescription,
        HasSiblings = s.HasSiblings,
        SiblingsCount = s.SiblingsCount,
        SiblingsGrades = s.SiblingsGrades,
        ParentObservations = s.ParentObservations,
        ReferralEmails = s.ReferralEmails,
        IsActive = s.IsActive,
        Guardians = s.Guardians?.Select(g => new StudentGuardianDto
        {
            Id = g.Id,
            Type = g.Type,
            IdentificationNumber = g.IdentificationNumber,
            FullName = g.FullName,
            Phones = g.Phones,
            Email = g.Email
        }).ToList() ?? new()
    };

    private static Student MapToEntity(StudentDto d) => new()
    {
        FirstName = d.FirstName,
        LastName = d.LastName,
        AccountNumber = d.AccountNumber,
        IdentificationNumber = d.IdentificationNumber,
        Gender = d.Gender,
        DateOfBirth = d.DateOfBirth,
        BirthPlace = d.BirthPlace,
        Address = d.Address,
        Handedness = d.Handedness,
        PhotoData = !string.IsNullOrEmpty(d.PhotoBase64) ? Convert.FromBase64String(d.PhotoBase64) : null,
        MedicalConditions = d.MedicalConditions,
        HasAllergies = d.HasAllergies,
        AllergiesDescription = d.AllergiesDescription,
        HasSiblings = d.HasSiblings,
        SiblingsCount = d.SiblingsCount,
        SiblingsGrades = d.SiblingsGrades,
        ParentObservations = d.ParentObservations,
        ReferralEmails = d.ReferralEmails
    };
}
