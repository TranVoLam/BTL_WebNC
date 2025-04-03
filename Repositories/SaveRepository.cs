using BTL_WebNC.Data;
using BTL_WebNC.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL_WebNC.Repositories;

public interface ISaveRepository {
    Task<List<Save>> GetByUserId(int userId);
    Task<int> SaveCount(int reviewId);
    Task Add(Save save);
    Task Delete(int id);
}

public class SaveRepository : ISaveRepository {
    private readonly WebNCDbContext _db;

    public SaveRepository(WebNCDbContext db) {
        _db = db;
    }

    
    public async Task<List<Save>> GetByUserId(int userId) 
    => await _db.Saves.Where(s => s.UserId == userId)
                            .AsNoTracking().ToListAsync();
    
    public async Task<int> SaveCount(int reviewId)
    {
        var count = await _db.Saves
        .Where(s => s.ReviewId == reviewId)
        .GroupBy(s => s.ReviewId)
        .Select(g => new { ReviewId = g.Key, Count = g.Count() })
        .AsNoTracking()
        .ToListAsync();

        return count.FirstOrDefault(s => s.ReviewId == reviewId)?.Count ?? 0;
    }

    public async Task Add(Save save) 
    {
        await _db.Saves.AddAsync(save);
        await _db.SaveChangesAsync();
        _db.Entry(save).State = EntityState.Detached;
    }

    public async Task Delete(int id) 
    {
        var save = await _db.Saves.FindAsync(id);
        if(save != null) {
            _db.Saves.Remove(save);
            await _db.SaveChangesAsync();
        }
    }
}