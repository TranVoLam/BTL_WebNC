using Microsoft.EntityFrameworkCore;
using BTL_WebNC.Data;
using BTL_WebNC.Models;

namespace BTL_WebNC.Repositories;

public interface IReportRepository {
    Task<List<Report>?> GetAll();
    Task<List<Report>?> GetByStatus(bool status);
    Task<List<Report>?> GetByReporterId(int reporterId);
    Task<List<Report>?> GeyByHandlerId(int handlerId);
    Task<List<Report>?> GetByCreateAt(DateTime createAt1, DateTime? createAt2 = null, byte? mode = null);
    Task<List<Report>?> GetByTarget(string targetType, int targetId);
    Task<List<Report>?> GetByResult(bool result);
    Task<List<Report>?> GetByResolvedAt(DateTime resolvedAt1, DateTime? resolvedAt2 = null, byte? mode = null);
}

public class ReportRepository : IReportRepository {
    private readonly WebNCDbContext _db;

    public ReportRepository(WebNCDbContext db) {
        _db = db;
    }
    public async Task<List<Report>?> GetAll() {
        var reports = await _db.Reports.AsNoTracking().ToListAsync();
        return reports.Count == 0 ? null : reports;
    }
    public async Task<List<Report>?> GetByStatus(bool status) {
        var reports = await _db.Reports.Where(r => r.ReportStatus == status)
                    .AsNoTracking().ToListAsync();
        return reports.Count == 0 ? null : reports;
    }
    public async Task<List<Report>?> GetByReporterId(int reporterId) {
        var reports = await _db.Reports.Where(r => r.ReporterId == reporterId)
                    .AsNoTracking().ToListAsync();
        return reports.Count == 0 ? null : reports;
    }
    public async Task<List<Report>?> GeyByHandlerId(int handlerId) {
        var reports = await _db.Reports.Where(r => r.HandlerId == handlerId)
                    .AsNoTracking().ToListAsync();
        return reports.Count == 0 ? null : reports;
    }
    public async Task<List<Report>?> GetByCreateAt
    (DateTime createAt1, DateTime? createAt2 = null, byte? mode = null) {
        var query = _db.Reports.AsQueryable();
        switch (mode) {
            case 1: // Trước thời điểm
                query = query.Where(r => r.CreateAt.Date <= createAt1.Date);
                break;
            case 2:  // Sau thời điểm
                query = query.Where(r => r.CreateAt.Date >= createAt1.Date);
                break;
            case 3: // GIữa 2 thời điểm
                if (createAt2.HasValue) {
                    query = query.Where(r => r.CreateAt.Date >= createAt1.Date 
                                        && r.CreateAt.Date <= createAt2.Value.Date);
                }
                break;
            default: // Đúng thời điểm
                query = query.Where(r => r.CreateAt.Date == createAt1.Date);
                break;
        }
        var reports = await query.AsNoTracking().ToListAsync();
        return reports.Any() ? reports : null;
    }
    public async Task<List<Report>?> GetByTarget(string targetType, int targetId) {
        var reports = await _db.Reports.Where(r => r.TargetType == targetType && r.TargetId == targetId)
                    .AsNoTracking().ToListAsync();
        return reports.Count == 0 ? null : reports;
    }
    public async Task<List<Report>?> GetByResult(bool result) {
        var reports = await _db.Reports.Where(r => r.Result == result)
                    .AsNoTracking().ToListAsync();
        return reports.Count == 0 ? null : reports;
    }
    public async Task<List<Report>?> GetByResolvedAt
    (DateTime resolvedAt1, DateTime? resolvedAt2 = null, byte? mode = null) {
        var query = _db.Reports.AsQueryable();
        switch (mode) {
            case 1: // Trước thời điểm
                query = query.Where(r => r.ResolvedAt.HasValue 
                        && r.ResolvedAt.Value.Date <= resolvedAt1.Date);
                break;
            case 2:  // Sau thời điểm
                query = query.Where(r => r.ResolvedAt.HasValue 
                        && r.ResolvedAt.Value.Date >= resolvedAt1.Date);
                break;
            case 3: // GIữa 2 thời điểm
                if (resolvedAt2.HasValue) {
                    query = query.Where(r => r.ResolvedAt.HasValue 
                            && r.ResolvedAt.Value.Date >= resolvedAt1.Date 
                                        && r.ResolvedAt.Value.Date <= resolvedAt2.Value.Date);
                }
                break;
            default: // Đúng thời điểm
                query = query.Where(r => r.ResolvedAt.HasValue 
                        && r.ResolvedAt.Value.Date == resolvedAt1.Date);
                break;
        }
        var reports = await query.AsNoTracking().ToListAsync();
        return reports.Any() ? reports : null;
    }
}