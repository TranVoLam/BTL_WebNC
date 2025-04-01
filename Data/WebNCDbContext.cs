using Microsoft.EntityFrameworkCore;

namespace BTL_WebNC.Data;

public class WebNCDbContext : DbContext {
    // DbSet<> ...;
    public WebNCDbContext(DbContextOptions<WebNCDbContext> options) : base(options) {}
}