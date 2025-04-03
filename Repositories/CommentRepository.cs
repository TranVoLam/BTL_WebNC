using System.Linq.Expressions;
using BTL_WebNC.Data;
using BTL_WebNC.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL_WebNC.Repositories;

public interface ICommentRepository {
    Task<Comment?> GetById(int id);
    Task<List<Comment>> GetByReviewId(int reviewId, int? afterId = null);
    Task<List<Comment>> GetByParentId(int reviewId, int parentId, int? afterId = null);
    Task<int> CommentCount(int ReviewId);
    Task Add(Comment comment);
    Task Update(int id, Comment comment);
    Task Delete(int id);
}

public class CommentRepository : ICommentRepository {
    private readonly WebNCDbContext _db;
    private const int Limit = 15;
    
    public CommentRepository(WebNCDbContext db) {
        _db = db;
    }

    private async Task<List<Comment>> GetComments
    (int reviewId, 
    int? afterId, 
    List<Expression<Func<Comment, bool>>>? filters = null)
    {
        var query = _db.Comments.Where(c => c.ReviewId == reviewId);
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
    
    public async Task<Comment?> GetById(int id) 
    => await _db.Comments.FindAsync(id);

    public Task<List<Comment>> GetByReviewId
    (int reviewId, 
    int? afterId = null
    ) 
    => GetComments(reviewId, afterId);

    public Task<List<Comment>> GetByParentId
    (int reviewId, 
    int parentId, 
    int? afterId = null)
    => GetComments(reviewId, afterId, new List<Expression<Func<Comment, bool>>> {
        c => c.ParentId == parentId
    });

    public async Task<int> CommentCount(int reviewId)
    {
        var count = await _db.Comments
        .Where(c => c.ReviewId == reviewId)
        .GroupBy(c => c.ReviewId)
        .Select(g => new {ReviewId = g.Key, Count = g.Count() })
        .AsNoTracking()
        .ToListAsync();

        return count.FirstOrDefault(c => c.ReviewId == reviewId)?.Count ?? 0;
    }

    public async Task Add(Comment comment) 
    {
        await _db.Comments.AddAsync(comment);
        await _db.SaveChangesAsync();
        _db.Entry(comment).State = EntityState.Detached;
    }

    public async Task Delete(int id) 
    {
        var comment = await _db.Comments.FindAsync(id);
        if (comment != null) {
            _db.Comments.Remove(comment);
            await _db.SaveChangesAsync();
        }
    }
    
    public async Task Update(int id, Comment comment) 
    {
        var _comment = await _db.Comments.FindAsync(id);
        if(_comment != null) {
            _db.Entry(_comment).CurrentValues.SetValues(comment);
            await _db.SaveChangesAsync();
            _db.Entry(_comment).State = EntityState.Detached;
        }
    }
}