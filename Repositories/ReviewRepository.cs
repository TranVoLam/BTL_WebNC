using BTL_WebNC.Data;
using BTL_WebNC.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL_WebNC.Repositories;

public interface IReviewRepository {
    Task<List<Review>?> GetAll();
    Task<Review?> GetById(int id);
    Task<List<Review>?> GetByUserId(int userId);
    Task<List<Review>?> GetByTitle(string keywords);
    Task<List<Review>?> GetByCreatAt(DateTime createAt1, DateTime? createAt2, byte mode = 0);
    Task<List<Review>?> GetByCategoryId(int categoryId);
    Task<List<Review>?> GetByRating(byte rating1, byte? rating2 = null, byte mode = 0);
    Task Add(Review review);
    Task Delete(int id);
    Task Update(int id, Review review);
}

public class ReviewRepository : IReviewRepository {
    private readonly WebNCDbContext _db;
    public ReviewRepository(WebNCDbContext db) {
        _db = db;
    } 
    public async Task<List<Review>?> GetAll() {
        var reviews = await _db.Reviews.AsNoTracking().ToListAsync();
        return reviews.Any() ? reviews : null;
    }
    public async Task<Review?> GetById(int id) {
        var review = await _db.Reviews.FindAsync(id);
        return review;
    }
    public async Task<List<Review>?> GetByUserId(int userId) {
        var reviews = await _db.Reviews.Where(r => r.UserId == userId)
                            .AsNoTracking().ToListAsync();
        return reviews.Any() ? reviews : null;
    }
    public async Task<List<Review>?> GetByTitle(string keywords) {
        var reviews = await _db.Reviews.Where(r => r.Title.Contains(keywords))
                                        .AsNoTracking().ToListAsync();
        return reviews.Any() ? reviews : null;
    }
    public async Task<List<Review>?> GetByCreatAt(
        DateTime createAt1 , DateTime? createAt2, byte mode = 0
        ) {
        var query = _db.Reviews.AsQueryable();
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
        var reviews = await query.AsNoTracking().ToListAsync();
        return reviews.Any() ? reviews : null;

    }
    public async Task<List<Review>?> GetByCategoryId(int categoryId) {
        var reviews = await _db.Reviews.Where(r => r.CategoryId == categoryId)
                                .AsNoTracking().ToListAsync();
        return reviews.Any() ? reviews : null;
    }    
    public async Task<List<Review>?> GetByRating(byte rating1, byte? rating2 = null, byte mode = 0) {
        var reviews = new List<Review>();
        if (mode == 0) { // Tìm đúng sao
            reviews = await _db.Reviews.Where(r => r.Rating == rating1)
                                        .AsNoTracking().ToListAsync();
        } else if(mode == 1) { // Tìm bài có sao nhỏ hơn hoặc bằng
            reviews = await _db.Reviews.Where(r => r.Rating <= rating1)
                                        .AsNoTracking().ToListAsync();
        } else if(mode == 2) { // Tìm bài có sao lớn hơn hoặc bằng
            reviews = await _db.Reviews.Where(r => r.Rating >= rating1)
                                        .AsNoTracking().ToListAsync();
        } else if(mode == 3 && rating2.HasValue) { // Tìm giữa 2 sao
            reviews = await _db.Reviews.Where(r => r.Rating >= rating1
                                                && r.Rating <= rating2.Value)
                                        .AsNoTracking().ToListAsync();
        }
        return reviews.Any() ? reviews : null;
    }
    public async Task Add(Review review) {
        await _db.Reviews.AddAsync(review);
        await _db.SaveChangesAsync();
        _db.Entry(review).State = EntityState.Detached;
    }
    public async Task Delete(int id) {
        var review = await _db.Reviews.FindAsync(id);
        if (review != null) {
            _db.Reviews.Remove(review);
            await _db.SaveChangesAsync();
        }
    }
    public async Task Update(int id, Review review) {
        var _review = await _db.Reviews.FindAsync(id);
        if (_review != null) {
            _db.Entry(_review).CurrentValues.SetValues(review);
            await _db.SaveChangesAsync();
            _db.Entry(_review).State = EntityState.Detached;
        }
    }
}