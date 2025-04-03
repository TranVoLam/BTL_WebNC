using Microsoft.EntityFrameworkCore;
using BTL_WebNC.Data;
using BTL_WebNC.Models;
using System.Linq.Expressions;

namespace BTL_WebNC.Repositories;

public interface IHistoryRepository {
    Task<List<History>> GetByUserId(int userId, int? afterId = null);
    Task<List<History>> GetByActionType
    (int userId, string actionType, int? afterId = null);
    Task<List<History>> GetByTarget
    (int userId, string targetType, int targetId, int? afterId = null);
    Task<List<History>> GetByCreateAt
    (int userId, DateTime createAt1, DateTime? createAt2 = null, byte mode = 0, 
    int? afterId = null);
    Task Add(History history);
    Task Delete(int id);
}

public class HistoryRepository : IHistoryRepository {
    private readonly WebNCDbContext _db;
    private const int Limit = 15;

    public HistoryRepository(WebNCDbContext db) {
        _db = db;
    }

    private async Task<List<History>> GetHistories
    (int userId, 
    int? afterId,
    List<Expression<Func<History,bool>>>? filters = null)
    {
        var query = _db.Histories.Where(h => h.UserId == userId);

        if (filters != null)
            foreach (var filter in filters)
                query.Where(filter);

        if (afterId.HasValue)
            query.Where(h => h.Id > afterId.Value);
        
        return await query
                .OrderByDescending(h => h.CreateAt)
                .Take(Limit)
                .AsNoTracking()
                .ToListAsync();
    }

    public Task<List<History>> GetByUserId
    (int userId, 
    int? afterId = null)
    => GetHistories(userId, afterId);
    
    public Task<List<History>> GetByActionType
    (int userId, 
    string actionType, 
    int? afterId = null)
    => GetHistories(userId, afterId, new List<Expression<Func<History,bool>>> {
        h => h.ActionType == actionType,
    }); 

    public Task<List<History>> GetByTarget
    (int userId, 
    string targetType, 
    int targetId, 
    int? afterId = null) 
    => GetHistories(userId, afterId, new List<Expression<Func<History,bool>>> {
        h => h.TargetType == targetType,
        h => h.TargetId == targetId
    });

    public Task<List<History>> GetByCreateAt
    (int userId, 
    DateTime createAt1, 
    DateTime? createAt2 = null, 
    byte mode = 0, 
    int? afterId = null) 
    {
        Expression<Func<History, bool>> time = h => true;
        switch (mode) {
            case 1: // Trước thời điểm
                time = h => h.CreateAt.Date <= createAt1.Date;
                break;
            case 2:  // Sau thời điểm
                time = h => h.CreateAt.Date >= createAt1.Date;
                break;
            case 3: // GIữa 2 thời điểm
                if (createAt2.HasValue) 
                {
                    time = h => h.CreateAt.Date >= createAt1.Date 
                                        && h.CreateAt.Date <= createAt2.Value.Date;
                }
                break;
            default: // Đúng thời điểm
                time = h => h.CreateAt.Date == createAt1.Date;
                break;
        }

        return GetHistories(userId, afterId, new List<Expression<Func<History,bool>>> {
            time
        });
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