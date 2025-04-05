using BTL_WebNC.Data;
using BTL_WebNC.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL_WebNC.Repositories;

public interface ISaveRepository {
    Task<List<Save>?> GetByUserId(int userId);
    Task Add(Save save);
    Task Delete(int id);
}

public class SaveRepository : ISaveRepository {
    private readonly WebNCDbContext _db;

    public SaveRepository(WebNCDbContext db) {
        _db = db;
    } 
    public async Task<List<Save>?> GetByUserId(int userId) {
        var saves = await _db.Saves.Where(s => s.UserId == userId)
                            .AsNoTracking().ToListAsync();
        return saves.Count == 0 ? null : saves;
    }
    public async Task Add(Save save) {
        await _db.Saves.AddAsync(save);
        await _db.SaveChangesAsync();
        _db.Entry(save).State = EntityState.Detached;
    }
    public async Task Delete(int id) {
        var save = await _db.Saves.FindAsync(id);
        if(save != null) {
            _db.Saves.Remove(save);
            await _db.SaveChangesAsync();
        }
    }
}