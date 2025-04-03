using BTL_WebNC.Data;
using BTL_WebNC.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL_WebNC.Repositories;

public interface IVoteRepository {
    Task<List<Vote>> GetByUserId(int userId);
    Task<(int upCount, int downCount)> GetByTarget(string targetType, int targetId);
    Task Add(Vote vote);
    Task Delete(int id);
    Task Update(int id, bool upDown);
}

public class VoteRepository : IVoteRepository {
    private readonly WebNCDbContext _db;

    public VoteRepository(WebNCDbContext db) {
        _db = db;
    }

    public async Task<List<Vote>> GetByUserId(int userId)
    => await _db.Votes.Where(v => v.UserId == userId)
                    .AsNoTracking().ToListAsync();

    public async Task<(int upCount, int downCount)> GetByTarget
    (string targetType, 
    int targetId) 
    {
        var counts = await _db.Votes
        .Where(v => v.TargetType == targetType && v.TargetId == targetId)
        .GroupBy(v => v.UpDown)
        .Select(g => new {UpDown = g.Key, Count = g.Count() })
        .AsNoTracking()
        .ToListAsync();

        var upCount = counts.FirstOrDefault(u => u.UpDown == true)?.Count ?? 0;
        var downCount = counts.FirstOrDefault(d => d.UpDown == false)?.Count ?? 0;
        
        return (upCount, downCount);
    }

    public async Task Add(Vote vote) 
    {
        await _db.Votes.AddAsync(vote);
        await _db.SaveChangesAsync();
        _db.Entry(vote).State = EntityState.Detached;
    }

    public async Task Delete(int id) 
    {
        var vote = await _db.Votes.FindAsync(id);
        if (vote != null) {
            _db.Votes.Remove(vote);
            await _db.SaveChangesAsync();
        }
    }
    public async Task Update(int id, bool upDown) 
    {
        var vote = await _db.Votes.FindAsync(id);
        if (vote != null) {
            vote.UpDown = upDown;
            await _db.SaveChangesAsync();
            _db.Entry(vote).State = EntityState.Detached;
        }
    }
}