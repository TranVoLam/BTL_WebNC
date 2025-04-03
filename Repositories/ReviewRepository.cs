using System.Linq.Expressions;
using BTL_WebNC.Data;
using BTL_WebNC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL_WebNC.Repositories;

public interface IReviewRepository {
    Task<List<Review>> GetAll(int? afterId);
    Task<Review?> GetById(int id);
    Task<List<Review>> GetByIds(List<int> ids, int? afterId);
    Task<List<Review>> GetByUserId(int userId, int? afterId);
    Task<List<Review>> GetByTitle(string keywords, int? afterId);
    Task<List<Review>> GetByUserIdAndCreatAt
    (int userId, DateTime createAt1, DateTime? createAt2, byte mode, int? afterId);
    Task<List<Review>> GetByCategoryId(int categoryId, int? afterId);
    Task<List<Review>> GetByRating
    (byte rating1, byte? rating2, byte mode, int? afterId);
    Task Add(Review review);
    Task Delete(int id);
    Task Update(int id, Review review);
}

public class ReviewRepository : IReviewRepository {
    private readonly WebNCDbContext _db;
    private const int Limit = 10;

    public ReviewRepository(WebNCDbContext db) {
        _db = db;
    }

    private async Task<List<Review>> GetReviews
    (int? afterId = null, 
    List<Expression<Func<Review, bool>>>? filters = null)
    {
        var query = _db.Reviews.AsQueryable();
        
        if (filters != null)
            foreach (var filter in filters)
                query = query.Where(filter);
        
        if (afterId.HasValue)
            query = query.Where(r => r.Id > afterId.Value);

        return await query
                .OrderByDescending(r => r.CreateAt)
                .Take(Limit)
                .AsNoTracking()
                .ToListAsync();
    }
    
    public Task<List<Review>> GetAll(int? afterId = null) 
    => GetReviews(afterId);

    public async Task<Review?> GetById(int id) 
    => await _db.Reviews.FindAsync(id);

    public Task<List<Review>> GetByIds(List<int> Ids, int? afterId = null)
    => GetReviews(afterId, new List<Expression<Func<Review, bool>>> {
        r => Ids.Contains(r.Id)
    });

    public Task<List<Review>> GetByUserId(int userId, int? afterId)
    => GetReviews(afterId, new List<Expression<Func<Review, bool>>> {
        r => r.UserId == userId
    });

    public Task<List<Review>> GetByTitle(string keywords, int? afterId)
    => GetReviews(afterId, new List<Expression<Func<Review, bool>>> {
        r => r.Title.Contains(keywords)
    });

    public Task<List<Review>> GetByUserIdAndCreatAt
    (int userId, 
    DateTime createAt1, 
    DateTime? createAt2, 
    byte mode = 0, 
    int? afterId = null) 
    {
        Expression<Func<Review, bool>> time = r => true;
        switch (mode) {
            case 1: // Trước thời điểm
                time = r => r.CreateAt.Date <= createAt1.Date;
                break;
            case 2:  // Sau thời điểm
                time = r => r.CreateAt.Date >= createAt1.Date;
                break;
            case 3: // GIữa 2 thời điểm
                if (createAt2.HasValue) {
                    time = r => r.CreateAt.Date >= createAt1.Date
                            && r.CreateAt.Date <= createAt2.Value.Date;
                }
                break;
            default: // Đúng thời điểm
                time = r => r.CreateAt.Date == createAt1.Date;
                break;
        }
        return GetReviews(afterId, new List<Expression<Func<Review, bool>>> {
            time
        });
    }

    public Task<List<Review>> GetByCategoryId
    (int categoryId, 
    int? afterId = null)
    => GetReviews(afterId, new List<Expression<Func<Review, bool>>> {
        r => r.CategoryId == categoryId
    });

    public Task<List<Review>> GetByRating
    (byte rating1, 
    byte? rating2 = null, 
    byte mode = 0, 
    int? afterId = null) 
    {
        Expression<Func<Review, bool>> rate = r => true;
        if (mode == 0) { // Tìm đúng sao
            rate = r => r.Rating == rating1;
        } else if(mode == 1) { // Tìm bài có sao nhỏ hơn hoặc bằng
            rate = r => r.Rating <= rating1;
        } else if(mode == 2) { // Tìm bài có sao lớn hơn hoặc bằng
            rate = r => r.Rating >= rating1;
        } else if(mode == 3 && rating2.HasValue) { // Tìm giữa 2 sao
            rate = r => r.Rating >= rating1 && r.Rating <= rating2.Value;
        }
        if (afterId.HasValue) {
            rate = r => r.Id > afterId.Value;
        }
        
        return GetReviews(afterId, new List<Expression<Func<Review, bool>>> {
            rate
        });
    }

    public async Task Add(Review review) 
    {
        await _db.Reviews.AddAsync(review);
        await _db.SaveChangesAsync();
        _db.Entry(review).State = EntityState.Detached;
    }
    public async Task Delete(int id) 
    {
        var review = await _db.Reviews.FindAsync(id);
        if (review != null) 
        {
            _db.Reviews.Remove(review);
            await _db.SaveChangesAsync();
        }
    }
    public async Task Update(int id, Review review) 
    {
        var _review = await _db.Reviews.FindAsync(id);
        if (_review != null) 
        {
            _db.Entry(_review).CurrentValues.SetValues(review);
            await _db.SaveChangesAsync();
            _db.Entry(_review).State = EntityState.Detached;
        }
    }
}