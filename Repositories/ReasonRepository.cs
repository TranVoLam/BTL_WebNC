using BTL_WebNC.Data;
using BTL_WebNC.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL_WebNC.Repositories;

public interface IReasonRepository {
    Task<List<Reason>?> GetAll();
    Task<int?> GetIdByName(string name);
}

public class ReasonRepository : IReasonRepository {
    private readonly WebNCDbContext _db;

    public ReasonRepository(WebNCDbContext db) {
        _db = db;
    } 
    public async Task<List<Reason>?> GetAll() {
        var reasons = await _db.Reasons.AsNoTracking().ToListAsync();
        if (reasons.Count == 0) {
            return null;
        }
        return reasons;
    }
    public async Task<int?> GetIdByName(string name) {
        var reason = await _db.Reasons.FirstOrDefaultAsync(c => c.ReasonName == name);
        if (reason == null) {
            return null;
        }
        return reason.Id;
    }
}