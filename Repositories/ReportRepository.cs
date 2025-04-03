using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BTL_WebNC.Data;
using BTL_WebNC.Models;

namespace BTL_WebNC.Repositories;

public interface IReportRepository {
    Task<List<Report>> GetAll();
    Task<Report?> GetById(int id);
    Task<List<Report>> GetByStatus(bool status, int? afterId);
    Task<List<Report>> GetByReporterId(int reporterId, int? afterId);
    Task<List<Report>> GeyByHandlerId(int handlerId, int? afterId);
    Task<List<Report>> GetByCreateAt
    (DateTime createAt1, DateTime? createAt2, byte mode, int? afterId);
    Task<List<Report>> GetByTarget(string targetType, int targetId, int? afterId);
    Task<List<Report>> GetByResult(bool result, int? afterId);
    Task<List<Report>> GetByResolvedAt
    (DateTime resolvedAt1, DateTime? resolvedAt2, byte mode, int? afterId);
    Task<int> ReportCount(string targetType, int targetId);
    Task Add(Report report);
    Task Delete(int id);
    Task Update(int id, Report report);
}

public class ReportRepository : IReportRepository {
    private readonly WebNCDbContext _db;
    private const int Limit = 15;

    public ReportRepository(WebNCDbContext db) {
        _db = db;
    }

    private async Task<List<Report>> GetReports
    (int? afterId, 
    List<Expression<Func<Report, bool>>>? filters = null)
    {
        var query = _db.Reports.AsQueryable();
        if (filters != null)
            foreach (var filter in filters)
                query = query.Where(filter);
    
        if (afterId.HasValue) 
            query = query.Where(c => c.Id > afterId.Value);

        return await query
            .OrderByDescending(h => h.CreateAt)
            .Take(Limit)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Report>> GetAll() 
    => await _db.Reports.AsNoTracking().ToListAsync();

    public async Task<Report?> GetById(int id)
    => await _db.Reports.FindAsync(id);

    public Task<List<Report>> GetByStatus
    (bool status, 
    int? afterId = null)
    => GetReports(afterId, new List<Expression<Func<Report, bool>>> {
        r => r.ReportStatus == status
    });
    
    public Task<List<Report>> GetByReporterId
    (int reporterId, 
    int? afterId = null)
    => GetReports(afterId, new List<Expression<Func<Report, bool>>> {
        r => r.ReporterId == reporterId
    });
    
    public Task<List<Report>> GeyByHandlerId
    (int handlerId, 
    int? afterId = null) 
    => GetReports(afterId, new List<Expression<Func<Report, bool>>> {
        r => r.HandlerId == handlerId
    });

    public Task<List<Report>> GetByCreateAt
    (DateTime createAt1, 
    DateTime? createAt2 = null, 
    byte mode = 0, 
    int? afterId = null) {
        Expression<Func<Report, bool>> time = r => true;
        switch (mode) {
            case 1: // Trước thời điểm
                time = r => r.CreateAt.Date <= createAt1.Date;
                break;
            case 2:  // Sau thời điểm
                time = r => r.CreateAt.Date >= createAt1.Date;
                break;
            case 3: // GIữa 2 thời điểm
                if (createAt2.HasValue) {
                    time = r => r.CreateAt.Date >= createAt1.Date
                        && r.CreateAt.Date <= createAt2.Value.Date;
                }
                break;
            default: // Đúng thời điểm
                time = r => r.CreateAt.Date == createAt1.Date;
                break;
        }

        return GetReports(afterId, new List<Expression<Func<Report, bool>>> {
            time
        });
    }

    public Task<List<Report>> GetByTarget
    (string targetType, 
    int targetId, 
    int? afterId = null)
    => GetReports(afterId, new List<Expression<Func<Report, bool>>> {
        r => r.TargetType == targetType,
        r => r.TargetId == targetId
    });

    public Task<List<Report>> GetByResult
    (bool result, 
    int? afterId = null)
    => GetReports(afterId, new List<Expression<Func<Report, bool>>> {
        r => r.Result == result
    });

    public Task<List<Report>> GetByResolvedAt
    (DateTime resolvedAt1, 
    DateTime? resolvedAt2 = null, 
    byte mode = 0,
    int? afterId = null) {
        Expression<Func<Report, bool>> time = r => true;
        switch (mode) {
            case 1: // Trước thời điểm
                time = r => r.ResolvedAt.HasValue 
                        && r.ResolvedAt.Value.Date <= resolvedAt1.Date;
                break;
            case 2:  // Sau thời điểm
                time = r => r.ResolvedAt.HasValue 
                        && r.ResolvedAt.Value.Date >= resolvedAt1.Date;
                break;
            case 3: // GIữa 2 thời điểm
                if (resolvedAt2.HasValue) {
                    time = r => r.ResolvedAt.HasValue 
                            && r.ResolvedAt.Value.Date >= resolvedAt1.Date
                                        && r.ResolvedAt.Value.Date <= resolvedAt2.Value.Date;
                }
                break;
            default: // Đúng thời điểm
                time = r => r.ResolvedAt.HasValue 
                        && r.ResolvedAt.Value.Date == resolvedAt1.Date;
                break;
        }
        return GetReports(afterId, new List<Expression<Func<Report, bool>>> {
            time
        });
    }

    public async Task<int> ReportCount(string targetType, int targetId)
    {
        var count = await _db.Votes
        .Where(r => r.TargetType == targetType && r.TargetId == targetId)
        .GroupBy(r => r.TargetId)
        .Select(g => new {TargetId = g.Key, Count = g.Count() })
        .AsNoTracking()
        .ToListAsync();

        return  count.FirstOrDefault(v => v.TargetId == targetId)?.Count ?? 0;
    }

    public async Task Add(Report report)
    {
        await _db.Reports.AddAsync(report);
        await _db.SaveChangesAsync();
        _db.Entry(report).State = EntityState.Detached;
    }

    public async Task Delete(int id)
    {
        var report = await _db.Reports.FindAsync(id);
        if (report != null)
        {
            _db.Reports.Remove(report);
            await _db.SaveChangesAsync();
        }
    }

    public async Task Update(int id, Report report)
    {
        var _report = await _db.Reports.FindAsync(id);
        if (_report != null)
        {
            _db.Entry(_report).CurrentValues.SetValues(report);
            await _db.SaveChangesAsync();
            _db.Entry(_report).State = EntityState.Detached;
        }
    }
}