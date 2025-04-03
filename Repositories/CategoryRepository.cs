using BTL_WebNC.Data;
using BTL_WebNC.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL_WebNC.Repositories;

public interface ICategoryRepository {
    Task<List<Category>> GetAll();
    Task<int?> GetIdByName(string name);
}

public class CategoryRepository : ICategoryRepository {
    private readonly WebNCDbContext _db;

    public CategoryRepository(WebNCDbContext db) {
        _db = db;
    } 

    public async Task<List<Category>> GetAll() 
    => await _db.Categories.AsNoTracking().ToListAsync();

    public async Task<int?> GetIdByName(string name)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(c => c.CategoryName == name);
        return category?.Id;
    }
}