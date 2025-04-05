using Microsoft.EntityFrameworkCore;
using BTL_WebNC.Data;
using BTL_WebNC.Models;

namespace BTL_WebNC.Repositories;

public interface IHistoryRepository {
    Task<List<History>?> GetByUserId(int userId);
    Task<List<History>?> GetByActionType(string actionType);
    Task<List<History>?> GetByTarget(string targetType, int targetId);
    Task<List<History>?> GetByCreateAt(DateTime createAt1, DateTime? createAt2 = null, byte mode = 0);
    Task Add(History history);
    Task Delete(int id);
}

public class HistoryRepository : IHistoryRepository {
    private readonly WebNCDbContext _db;

    public HistoryRepository(WebNCDbContext db) {
        _db = db;
    }
    public async Task<List<History>?> GetByUserId(int userId) {
        var histories = await _db.Histories.Where(h => h.UserId == userId)
                        .AsNoTracking().ToListAsync();
        return histories.Any() ? histories : null;
    }
    public async Task<List<History>?> GetByActionType(string actionType) {
        var histories = await _db.Histories.Where(h => h.ActionType == actionType)
                        .AsNoTracking().ToListAsync();
        return histories.Any() ? histories : null;
    }
    public async Task<List<History>?> GetByTarget(string targetType, int targetId) {
        var histories = await _db.Histories.Where(h => h.TargetType == targetType && h.TargetId == targetId)
                        .AsNoTracking().ToListAsync();
        return histories.Any() ? histories : null;
    }
    public async Task<List<History>?> GetByCreateAt(
        DateTime createAt1 , DateTime? createAt2 = null, byte mode = 0
        ) {
        var query = _db.Histories.AsQueryable();
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
        var histories = await query.AsNoTracking().ToListAsync();
        return histories.Any() ? histories : null;
    }
    public async Task Add(History history) {
        await _db.Histories.AddAsync(history);
        await _db.SaveChangesAsync();
        _db.Entry(history).State = EntityState.Detached;
    }
    public async Task Delete(int id) {
        var history = await _db.Histories.FindAsync(id);
        if (history != null) {
            _db.Histories.Remove(history);
            await _db.SaveChangesAsync();
        }    
    }
}