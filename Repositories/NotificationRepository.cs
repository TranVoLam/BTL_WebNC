using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BTL_WebNC.Data;
using BTL_WebNC.Models;

namespace BTL_WebNC.Repositories;

public interface INotificationRepository {
    Task<List<Notification>> GetByUserId(int userId, int? afterId);
    Task<List<Notification>> GetByNotificationType
    (int userId, string notificationType, int? afterId);
    Task<List<Notification>> GetByTarget
    (int userId, string targetType, int targetId, int? afterId);
    Task<List<Notification>> GetByCreateAt
    (int userId, DateTime createAt1, DateTime? createAt2, byte mode, int? afterId);
    Task<List<Notification>> GetByIsRead(int userId, bool isRead, int? afterId);
    Task Add(Notification notification);
    Task Delete(int id);
}

public class NotificationRepository : INotificationRepository {
    private readonly WebNCDbContext _db;
    private const int Limit = 15;
    public NotificationRepository(WebNCDbContext db) {
        _db = db;
    }

    private async Task<List<Notification>> GetNotifications
    (int userId, 
    int? afterId, 
    List<Expression<Func<Notification, bool>>>? filters = null)
    {
        var query = _db.Notifications.Where(n => n.UserId == userId);
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

    public Task<List<Notification>> GetByUserId(int userId, int? afterId = null) 
    => GetNotifications(userId, afterId);

    public Task<List<Notification>> GetByNotificationType
    (int userId, 
    string notificationType, 
    int? afterId = null) 
    => GetNotifications(userId, afterId, new List<Expression<Func<Notification, bool>>> {
        n => n.NotificationType == notificationType
    });

    public Task<List<Notification>> GetByTarget
    (int userId, 
    string targetType, 
    int targetId,
    int? afterId = null) 
    => GetNotifications(userId, afterId, new List<Expression<Func<Notification, bool>>> {
        n => n.TargetType == targetType,
        n => n.TargetId == targetId
    });

    public Task<List<Notification>> GetByCreateAt
    (int userId, 
    DateTime createAt1, 
    DateTime? createAt2 = null, 
    byte mode = 0,
    int? afterId = null) 
    {
        Expression<Func<Notification, bool>> time = n => true;
        switch (mode) {
            case 1: // Trước thời điểm
                time = n => n.CreateAt.Date <= createAt1.Date;
                break;
            case 2:  // Sau thời điểm
                time = n => n.CreateAt.Date >= createAt1.Date;
                break;
            case 3: // GIữa 2 thời điểm
                if (createAt2.HasValue) {
                    time = n => n.CreateAt.Date >= createAt1.Date
                        && n.CreateAt.Date <= createAt2.Value.Date;
                }
                break;
            default: // Đúng thời điểm
                time = n => n.CreateAt.Date == createAt1.Date;
                break;
        }
        
        return GetNotifications(userId, afterId, new List<Expression<Func<Notification, bool>>> {
            time
        });
    }

    public Task<List<Notification>> GetByIsRead
    (int userId, 
    bool isRead, 
    int? afterId = null) 
    => GetNotifications(userId, afterId, new List<Expression<Func<Notification, bool>>> {
            n => n.IsRead == isRead
        });
    public async Task Add(Notification notification) 
    {
        await _db.Notifications.AddAsync(notification);
        await _db.SaveChangesAsync();
        _db.Entry(notification).State = EntityState.Detached;
    }
    public async Task Delete(int id) 
    {
        var notifications = await _db.Notifications.FindAsync(id);
        if (notifications != null) {
            _db.Notifications.Remove(notifications);
            await _db.SaveChangesAsync();
        }
    }
}