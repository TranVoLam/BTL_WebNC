using BTL_WebNC.Data;
using BTL_WebNC.Models;
using Microsoft.EntityFrameworkCore;
namespace BTL_WebNC.Repositories;

public interface IUserRepository {
    Task<List<User>?> GetAll();
    Task<User?> GetById(int id);
    Task<User?> GetByEmail(string email);
    Task<List<User>?> GetByFullName(string keywords);
    Task<List<User>?> GetByUserStatus(bool userStatus);
    Task<List<User>?> GetByCreateAt(DateTime createAt1, DateTime? createAt2 = null, byte mode = 0);
    Task Add(User user);
    Task Delete(int id);
    Task Update(int id, User user);
}

public class UserRepository : IUserRepository {
    private readonly WebNCDbContext _db;

    public UserRepository(WebNCDbContext db) {
        _db = db;
    }
    public async Task<List<User>?> GetAll() {
        var users = await _db.Users.AsNoTracking().ToListAsync();
        return users.Any() ? users : null;
    }
    public async Task<User?> GetById(int id) {
        var user = await _db.Users.FindAsync(id);
        return user;
    }
    public async Task<User?> GetByEmail(string email) {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user;
    }
    public async Task<List<User>?> GetByFullName(string keywords) {
        var users = await _db.Users
        .Where(u => u != null && u.FullName != null && u.FullName.Contains(keywords))
        .AsNoTracking().ToListAsync();
        
        return users.Any() ? users : null;
    }
    public async Task<List<User>?> GetByUserStatus(bool userStatus) {
        var users = await _db.Users.Where(u => u.UserStatus == userStatus)
                                    .AsNoTracking().ToListAsync();
        return users.Any() ? users : null;
    }
    public async Task Add(User user) {
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
        _db.Entry(user).State = EntityState.Detached;
    }
    public async Task Delete(int id) {
        var user = await _db.Users.FindAsync(id);
        if(user != null) {
            _db.Users.Remove(user);
        }
    }
    public async  Task Update(int id, User user) {
        var _user = await _db.Users.FindAsync(id);
        if (_user != null) {
            _db.Entry(_user).CurrentValues.SetValues(user);
            await _db.SaveChangesAsync();
            _db.Entry(_user).State = EntityState.Detached;
        }
    }
    public async Task<List<User>?> GetByCreateAt
    (DateTime createAt1, DateTime? createAt2 = null, byte mode = 0) {
        var query = _db.Users.AsQueryable();
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
        var users = await query.AsNoTracking().ToListAsync();
        return users.Any() ? users : null;
    }
}