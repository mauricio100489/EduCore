using EduCore.Application.DTOs;
using EduCore.Application.Interfaces;
using EduCore.Domain.Entities;
using EduCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduCore.Infrastructure.Services;

// ===== InstituteService =====
public class InstituteService : IInstituteService
{
    private readonly EduCoreDbContext _context;
    public InstituteService(EduCoreDbContext context) => _context = context;

    public async Task<IEnumerable<InstituteDto>> GetAllAsync()
    {
        var entities = await _context.Institutes.Where(e => e.IsActive).ToListAsync();
        return entities.Select(MapToDto);
    }

    public async Task<InstituteDto?> GetByIdAsync(int id)
    {
        var entity = await _context.Institutes.FindAsync(id);
        return entity is null ? null : MapToDto(entity);
    }

    public async Task<InstituteDto> CreateAsync(InstituteDto dto)
    {
        var entity = MapToEntity(dto);
        entity.CreatedAt = DateTime.UtcNow;
        _context.Institutes.Add(entity);
        await _context.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task UpdateAsync(InstituteDto dto)
    {
        var entity = await _context.Institutes.FindAsync(dto.Id)
            ?? throw new KeyNotFoundException($"Institute with Id {dto.Id} not found.");
        entity.Code = dto.Code;
        entity.Rtn = dto.Rtn;
        entity.Name = dto.Name;
        entity.Slogan = dto.Slogan;
        entity.Address = dto.Address;
        entity.Phones = dto.Phones;
        entity.Director = dto.Director;
        entity.Secretary = dto.Secretary;
        entity.IsActive = dto.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Institutes.FindAsync(id);
        if (entity != null) { entity.IsActive = false; await _context.SaveChangesAsync(); }
    }

    private static InstituteDto MapToDto(Institute e) => new()
    {
        Id = e.Id, Code = e.Code, Rtn = e.Rtn, Name = e.Name,
        Slogan = e.Slogan, Address = e.Address, Phones = e.Phones,
        Director = e.Director, Secretary = e.Secretary, IsActive = e.IsActive
    };

    private static Institute MapToEntity(InstituteDto d) => new()
    {
        Code = d.Code, Rtn = d.Rtn, Name = d.Name, Slogan = d.Slogan,
        Address = d.Address, Phones = d.Phones, Director = d.Director,
        Secretary = d.Secretary, IsActive = d.IsActive
    };
}

// ===== InstitutePeriodService =====
public class InstitutePeriodService : IInstitutePeriodService
{
    private readonly EduCoreDbContext _context;
    public InstitutePeriodService(EduCoreDbContext context) => _context = context;

    public async Task<IEnumerable<InstitutePeriodDto>> GetAllAsync()
    {
        var entities = await _context.InstitutePeriods.Include(p => p.Institute)
            .Where(p => p.IsActive).ToListAsync();
        return entities.Select(MapToDto);
    }

    public async Task<IEnumerable<InstitutePeriodDto>> GetByInstituteAsync(int instituteId)
    {
        var entities = await _context.InstitutePeriods.Include(p => p.Institute)
            .Where(p => p.InstituteId == instituteId && p.IsActive).ToListAsync();
        return entities.Select(MapToDto);
    }

    public async Task<InstitutePeriodDto?> GetByIdAsync(int id)
    {
        var entity = await _context.InstitutePeriods.Include(p => p.Institute)
            .FirstOrDefaultAsync(p => p.Id == id);
        return entity is null ? null : MapToDto(entity);
    }

    public async Task<InstitutePeriodDto> CreateAsync(InstitutePeriodDto dto)
    {
        var entity = MapToEntity(dto);
        entity.CreatedAt = DateTime.UtcNow;
        _context.InstitutePeriods.Add(entity);
        await _context.SaveChangesAsync();
        var created = await _context.InstitutePeriods.Include(p => p.Institute)
            .FirstAsync(p => p.Id == entity.Id);
        return MapToDto(created);
    }

    public async Task UpdateAsync(InstitutePeriodDto dto)
    {
        var entity = await _context.InstitutePeriods.FindAsync(dto.Id)
            ?? throw new KeyNotFoundException($"InstitutePeriod with Id {dto.Id} not found.");
        entity.PeriodName = dto.PeriodName;
        entity.InstituteId = dto.InstituteId;
        entity.StartDate = dto.StartDate;
        entity.EndDate = dto.EndDate;
        entity.PromotionName = dto.PromotionName;
        entity.IsCurrent = dto.IsCurrent;
        entity.IsActive = dto.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.InstitutePeriods.FindAsync(id);
        if (entity != null) { entity.IsActive = false; await _context.SaveChangesAsync(); }
    }

    private static InstitutePeriodDto MapToDto(InstitutePeriod e) => new()
    {
        Id = e.Id, PeriodName = e.PeriodName, InstituteId = e.InstituteId,
        InstituteName = e.Institute?.Name, StartDate = e.StartDate, EndDate = e.EndDate,
        PromotionName = e.PromotionName, IsCurrent = e.IsCurrent, IsActive = e.IsActive
    };

    private static InstitutePeriod MapToEntity(InstitutePeriodDto d) => new()
    {
        PeriodName = d.PeriodName, InstituteId = d.InstituteId,
        StartDate = d.StartDate, EndDate = d.EndDate,
        PromotionName = d.PromotionName, IsCurrent = d.IsCurrent, IsActive = d.IsActive
    };
}

// ===== InstituteGradeService =====
public class InstituteGradeService : IInstituteGradeService
{
    private readonly EduCoreDbContext _context;
    public InstituteGradeService(EduCoreDbContext context) => _context = context;

    private IQueryable<InstituteGrade> Query() => _context.InstituteGrades
        .Include(g => g.Institute).Include(g => g.Period);

    public async Task<IEnumerable<InstituteGradeDto>> GetAllAsync()
    {
        var entities = await Query().Where(g => g.IsActive).ToListAsync();
        return entities.Select(MapToDto);
    }

    public async Task<IEnumerable<InstituteGradeDto>> GetByInstituteAndPeriodAsync(int instituteId, int periodId)
    {
        var entities = await Query()
            .Where(g => g.InstituteId == instituteId && g.PeriodId == periodId && g.IsActive)
            .OrderBy(g => g.Order).ToListAsync();
        return entities.Select(MapToDto);
    }

    public async Task<InstituteGradeDto?> GetByIdAsync(int id)
    {
        var entity = await Query().FirstOrDefaultAsync(g => g.Id == id);
        return entity is null ? null : MapToDto(entity);
    }

    public async Task<InstituteGradeDto> CreateAsync(InstituteGradeDto dto)
    {
        var entity = MapToEntity(dto);
        entity.CreatedAt = DateTime.UtcNow;
        _context.InstituteGrades.Add(entity);
        await _context.SaveChangesAsync();
        var created = await Query().FirstAsync(g => g.Id == entity.Id);
        return MapToDto(created);
    }

    public async Task UpdateAsync(InstituteGradeDto dto)
    {
        var entity = await _context.InstituteGrades.FindAsync(dto.Id)
            ?? throw new KeyNotFoundException($"InstituteGrade with Id {dto.Id} not found.");
        entity.InstituteId = dto.InstituteId;
        entity.PeriodId = dto.PeriodId;
        entity.GradeName = dto.GradeName;
        entity.OfficialGrade = dto.OfficialGrade;
        entity.OfficialModality = dto.OfficialModality;
        entity.Order = dto.Order;
        entity.EnrollmentFee = dto.EnrollmentFee;
        entity.MonthlyFee12 = dto.MonthlyFee12;
        entity.MonthlyFee10 = dto.MonthlyFee10;
        entity.MaterialsFee = dto.MaterialsFee;
        entity.OtherFees = dto.OtherFees;
        entity.EvaluationType = dto.EvaluationType;
        entity.EmailSender = dto.EmailSender;
        entity.EmailName = dto.EmailName;
        entity.SmtpServer = dto.SmtpServer;
        entity.SmtpPort = dto.SmtpPort;
        entity.EmailTitle = dto.EmailTitle;
        entity.CounselorName = dto.CounselorName;
        entity.EducationLevelId = dto.EducationLevelId;
        entity.IsActive = dto.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.InstituteGrades.FindAsync(id);
        if (entity != null) { entity.IsActive = false; await _context.SaveChangesAsync(); }
    }

    private static InstituteGradeDto MapToDto(InstituteGrade e) => new()
    {
        Id = e.Id, InstituteId = e.InstituteId, PeriodId = e.PeriodId,
        InstituteName = e.Institute?.Name, PeriodName = e.Period?.PeriodName,
        GradeName = e.GradeName, OfficialGrade = e.OfficialGrade,
        OfficialModality = e.OfficialModality, Order = e.Order,
        EnrollmentFee = e.EnrollmentFee, MonthlyFee12 = e.MonthlyFee12,
        MonthlyFee10 = e.MonthlyFee10, MaterialsFee = e.MaterialsFee,
        OtherFees = e.OtherFees, EvaluationType = e.EvaluationType,
        EmailSender = e.EmailSender, EmailName = e.EmailName,
        SmtpServer = e.SmtpServer, SmtpPort = e.SmtpPort,
        EmailTitle = e.EmailTitle, CounselorName = e.CounselorName,
        EducationLevelId = e.EducationLevelId, IsActive = e.IsActive
    };

    private static InstituteGrade MapToEntity(InstituteGradeDto d) => new()
    {
        InstituteId = d.InstituteId, PeriodId = d.PeriodId,
        GradeName = d.GradeName, OfficialGrade = d.OfficialGrade,
        OfficialModality = d.OfficialModality, Order = d.Order,
        EnrollmentFee = d.EnrollmentFee, MonthlyFee12 = d.MonthlyFee12,
        MonthlyFee10 = d.MonthlyFee10, MaterialsFee = d.MaterialsFee,
        OtherFees = d.OtherFees, EvaluationType = d.EvaluationType,
        EmailSender = d.EmailSender, EmailName = d.EmailName,
        SmtpServer = d.SmtpServer, SmtpPort = d.SmtpPort,
        EmailTitle = d.EmailTitle, CounselorName = d.CounselorName,
        EducationLevelId = d.EducationLevelId, IsActive = d.IsActive
    };
}

// ===== InstituteShiftService =====
public class InstituteShiftService : IInstituteShiftService
{
    private readonly EduCoreDbContext _context;
    public InstituteShiftService(EduCoreDbContext context) => _context = context;

    private IQueryable<InstituteShift> Query() => _context.InstituteShifts
        .Include(s => s.Institute).Include(s => s.Period);

    public async Task<IEnumerable<InstituteShiftDto>> GetAllAsync()
    {
        var entities = await Query().Where(s => s.IsActive).ToListAsync();
        return entities.Select(MapToDto);
    }

    public async Task<IEnumerable<InstituteShiftDto>> GetByInstituteAndPeriodAsync(int instituteId, int periodId)
    {
        var entities = await Query()
            .Where(s => s.InstituteId == instituteId && s.PeriodId == periodId && s.IsActive)
            .OrderBy(s => s.Order).ToListAsync();
        return entities.Select(MapToDto);
    }

    public async Task<InstituteShiftDto?> GetByIdAsync(int id)
    {
        var entity = await Query().FirstOrDefaultAsync(s => s.Id == id);
        return entity is null ? null : MapToDto(entity);
    }

    public async Task<InstituteShiftDto> CreateAsync(InstituteShiftDto dto)
    {
        var entity = MapToEntity(dto);
        entity.CreatedAt = DateTime.UtcNow;
        _context.InstituteShifts.Add(entity);
        await _context.SaveChangesAsync();
        var created = await Query().FirstAsync(s => s.Id == entity.Id);
        return MapToDto(created);
    }

    public async Task UpdateAsync(InstituteShiftDto dto)
    {
        var entity = await _context.InstituteShifts.FindAsync(dto.Id)
            ?? throw new KeyNotFoundException($"InstituteShift with Id {dto.Id} not found.");
        entity.ShiftName = dto.ShiftName;
        entity.InstituteId = dto.InstituteId;
        entity.PeriodId = dto.PeriodId;
        entity.Order = dto.Order;
        entity.IsActive = dto.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.InstituteShifts.FindAsync(id);
        if (entity != null) { entity.IsActive = false; await _context.SaveChangesAsync(); }
    }

    private static InstituteShiftDto MapToDto(InstituteShift e) => new()
    {
        Id = e.Id, ShiftName = e.ShiftName, InstituteId = e.InstituteId,
        PeriodId = e.PeriodId, InstituteName = e.Institute?.Name,
        PeriodName = e.Period?.PeriodName, Order = e.Order, IsActive = e.IsActive
    };

    private static InstituteShift MapToEntity(InstituteShiftDto d) => new()
    {
        ShiftName = d.ShiftName, InstituteId = d.InstituteId,
        PeriodId = d.PeriodId, Order = d.Order, IsActive = d.IsActive
    };
}

// ===== InstituteSectionService =====
public class InstituteSectionService : IInstituteSectionService
{
    private readonly EduCoreDbContext _context;
    public InstituteSectionService(EduCoreDbContext context) => _context = context;

    private IQueryable<InstituteSection> Query() => _context.InstituteSections
        .Include(s => s.Institute).Include(s => s.Period);

    public async Task<IEnumerable<InstituteSectionDto>> GetAllAsync()
    {
        var entities = await Query().Where(s => s.IsActive).ToListAsync();
        return entities.Select(MapToDto);
    }

    public async Task<IEnumerable<InstituteSectionDto>> GetByInstituteAndPeriodAsync(int instituteId, int periodId)
    {
        var entities = await Query()
            .Where(s => s.InstituteId == instituteId && s.PeriodId == periodId && s.IsActive)
            .OrderBy(s => s.Order).ToListAsync();
        return entities.Select(MapToDto);
    }

    public async Task<InstituteSectionDto?> GetByIdAsync(int id)
    {
        var entity = await Query().FirstOrDefaultAsync(s => s.Id == id);
        return entity is null ? null : MapToDto(entity);
    }

    public async Task<InstituteSectionDto> CreateAsync(InstituteSectionDto dto)
    {
        var entity = MapToEntity(dto);
        entity.CreatedAt = DateTime.UtcNow;
        _context.InstituteSections.Add(entity);
        await _context.SaveChangesAsync();
        var created = await Query().FirstAsync(s => s.Id == entity.Id);
        return MapToDto(created);
    }

    public async Task UpdateAsync(InstituteSectionDto dto)
    {
        var entity = await _context.InstituteSections.FindAsync(dto.Id)
            ?? throw new KeyNotFoundException($"InstituteSection with Id {dto.Id} not found.");
        entity.SectionName = dto.SectionName;
        entity.InstituteId = dto.InstituteId;
        entity.PeriodId = dto.PeriodId;
        entity.Order = dto.Order;
        entity.IsActive = dto.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.InstituteSections.FindAsync(id);
        if (entity != null) { entity.IsActive = false; await _context.SaveChangesAsync(); }
    }

    private static InstituteSectionDto MapToDto(InstituteSection e) => new()
    {
        Id = e.Id, SectionName = e.SectionName, InstituteId = e.InstituteId,
        PeriodId = e.PeriodId, InstituteName = e.Institute?.Name,
        PeriodName = e.Period?.PeriodName, Order = e.Order, IsActive = e.IsActive
    };

    private static InstituteSection MapToEntity(InstituteSectionDto d) => new()
    {
        SectionName = d.SectionName, InstituteId = d.InstituteId,
        PeriodId = d.PeriodId, Order = d.Order, IsActive = d.IsActive
    };
}
