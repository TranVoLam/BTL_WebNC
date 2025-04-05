using BTL_WebNC.Data;
using BTL_WebNC.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL_WebNC.Repositories;

public interface ICommentRepository {
    Task<List<Comment>?> GetByUserId(int userId);
    Task<List<Comment>?> GetByReviewId(int reviewId);
    Task<List<Comment>?> GetByParentId(int parentId);
    Task<List<Comment>?> GetByTimeStorted(byte? mode = null);
    Task Add(Comment comment);
    Task Update(int id, Comment comment);
    Task Delete(int id);
}

public class CommentRepository : ICommentRepository {
    private readonly WebNCDbContext _db;

    public CommentRepository(WebNCDbContext db) {
        _db = db;
    }
    public async Task<List<Comment>?> GetByUserId(int userId) {
        var comments = await _db.Comments.Where(c => c.UserId == userId)
                        .AsNoTracking().ToListAsync();
        return comments.Any() ? comments : null;
    }
    public async Task<List<Comment>?> GetByReviewId(int reviewId) {
        var comments = await _db.Comments.Where(c => c.ReviewId == reviewId)
                        .AsNoTracking().ToListAsync();
        return comments.Any() ? comments : null;
    }
    public async Task<List<Comment>?> GetByParentId(int parentId) {
        var comments = await _db.Comments.Where(c => c.ParentId == parentId)
                        .AsNoTracking().ToListAsync();
        return comments.Any() ? comments : null;
    }
    public async Task<List<Comment>?> GetByTimeStorted(byte? mode = null) {
        var comments = new List<Comment>();
        switch (mode) {
            case 0:  // Sắp xếp theo tự tự cũ nhất
                comments = await _db.Comments.OrderBy(c => c.CreateAt)
                            .AsNoTracking().ToListAsync();
                break;
            default: // Sắp xếp theo thứ tự mới nhất
                comments = await _db.Comments.OrderByDescending(c => c.CreateAt)
                            .AsNoTracking().ToListAsync();
                break;
        }
        return comments.Any() ? comments : null;
    }
    public async Task Add(Comment comment) {
        await _db.Comments.AddAsync(comment);
        await _db.SaveChangesAsync();
        _db.Entry(comment).State = EntityState.Detached;
    }
    public async Task Delete(int id) {
        var comment = await _db.Comments.FindAsync(id);
        if (comment != null) {
            _db.Comments.Remove(comment);
            await _db.SaveChangesAsync();
        }
    }
    public async Task Update(int id, Comment comment) {
        var _comment = await _db.Comments.FindAsync(id);
        if(_comment != null) {
            _db.Entry(_comment).CurrentValues.SetValues(comment);
            await _db.SaveChangesAsync();
            _db.Entry(_comment).State = EntityState.Detached;
        }
    }
}