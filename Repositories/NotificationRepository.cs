using Microsoft.EntityFrameworkCore;
using BTL_WebNC.Data;
using BTL_WebNC.Models;

namespace BTL_WebNC.Repositories;

public interface INotificationRepository {
    Task<List<Notification>?> GetByUserId(int userId);
    Task<List<Notification>?> GetByNotificationType(string notificationType);
    Task<List<Notification>?> GetByTarget(string targetType, int targetId);
    Task<List<Notification>?> GetByCreateAt(DateTime createAt1, DateTime? createAt2 = null, byte? mode = null);
    Task<List<Notification>?> GetByIsRead(bool isRead);
    Task Add(Notification notification);
    Task Delete(int id);
}

public class NotificationRepository : INotificationRepository {
    private readonly WebNCDbContext _db;

    public NotificationRepository(WebNCDbContext db) {
        _db = db;
    }
    public async Task<List<Notification>?> GetByUserId(int userId) {
        var notifications = await _db.Notifications.Where(n => n.UserId == userId)
                            .AsNoTracking().ToListAsync();
        return notifications.Any() ? notifications : null;
    }
    public async Task<List<Notification>?> GetByNotificationType(string notificationType) {
        var notifications = await _db.Notifications.Where(n => n.NotificationType == notificationType)
                            .AsNoTracking().ToListAsync();
        return notifications.Any() ? notifications : null;
    }
    public async Task<List<Notification>?> GetByTarget(string targetType, int targetId) {
        var notifications = await _db.Notifications
                            .Where(n => n.TargetType == targetType && n.TargetId == targetId)
                            .AsNoTracking().ToListAsync();
        return notifications.Any() ? notifications : null;
    }
    public async Task<List<Notification>?> GetByCreateAt
    (DateTime createAt1, DateTime? createAt2 = null, byte? mode = null) {
        var query = _db.Notifications.AsQueryable();
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
        var notifications = await query.AsNoTracking().ToListAsync();
        return notifications.Any() ? notifications : null;
    }
    public async Task<List<Notification>?> GetByIsRead(bool isRead) {
        var notifications = await _db.Notifications.Where(n => n.IsRead == isRead)
                            .AsNoTracking().ToListAsync();
        return notifications.Any() ? notifications : null;
    }
    public async Task Add(Notification notification) {
        await _db.Notifications.AddAsync(notification);
        await _db.SaveChangesAsync();
        _db.Entry(notification).State = EntityState.Detached;
    }
    public async Task Delete(int id) {
        var notifications = await _db.Notifications.FindAsync(id);
        if (notifications != null) {
            _db.Notifications.Remove(notifications);
            await _db.SaveChangesAsync();
        }
    }
}