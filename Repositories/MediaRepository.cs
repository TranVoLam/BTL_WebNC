using BTL_WebNC.Data;
using BTL_WebNC.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL_WebNC.Repositories;

public interface IMediaRepository {
    Task<List<Media>?> GetByReviewId(int reviewId);
    Task Add(Media media);
    Task Delete(int id);
}

public class MediaRepository : IMediaRepository {
    private readonly WebNCDbContext _db;

    public MediaRepository(WebNCDbContext db) {
        _db = db;
    } 

    public async Task<List<Media>?> GetByReviewId(int id) {
        var medias = await _db.Medias.Where(m => m.ReviewId == id).AsNoTracking().ToListAsync();
        return medias.Count == 0 ? null : medias;
    }

    public async Task Add(Media media) {
        await _db.Medias.AddAsync(media);
        await _db.SaveChangesAsync();
        _db.Entry(media).State = EntityState.Detached;
    }

    public async Task Delete(int id) {
        var media = await _db.Medias.FindAsync(id);
        if (media != null) {
            _db.Medias.Remove(media);
            await _db.SaveChangesAsync();
        }
    }
}