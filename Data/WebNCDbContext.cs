using BTL_WebNC.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL_WebNC.Data;

public class WebNCDbContext : DbContext {
    public WebNCDbContext(DbContextOptions<WebNCDbContext> options) : base(options) {}
    
    public DbSet<User> Users { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Media> Medias { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Reason> Reasons { get; set; }
    public DbSet<Vote> Votes { get; set; }
    public DbSet<Save> Saves { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<History> Histories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // User
        modelBuilder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();

        modelBuilder.Entity<User>()
        .Property(u => u.CreateAt)
        .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<User>()
        .ToTable(table => table.HasCheckConstraint("CK_Users_Sex", "Sex IN (0, 1, 2)"));

        modelBuilder.Entity<User>()
        .Property(u => u.UserRole)
        .HasDefaultValue(false);

        modelBuilder.Entity<User>()
        .Property(u => u.UserStatus)
        .HasDefaultValue(false);

        // Review
        modelBuilder.Entity<Review>()
        .HasOne(re => re.User)
        .WithMany(u => u.Reviews)
        .HasForeignKey(re => re.UserId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Review>()
        .HasOne(re => re.Category)
        .WithMany(c => c.Reviews)
        .HasForeignKey(re => re.CategoryId);

        modelBuilder.Entity<Review>()
        .Property(re => re.CreateAt)
        .HasDefaultValueSql("GETUTCDATE()");

        // Media
        modelBuilder.Entity<Media>()
        .HasOne(m => m.Review)
        .WithMany(re => re.Medias)
        .HasForeignKey(m => m.ReviewId)
        .OnDelete(DeleteBehavior.Cascade);

        // Report
        modelBuilder.Entity<Report>()
        .HasOne(rp => rp.User)
        .WithMany(u => u.Reports)
        .HasForeignKey(rp => rp.ReporterId);

        modelBuilder.Entity<Report>()
        .HasOne(rp => rp.User)
        .WithMany(u => u.Reports)
        .HasForeignKey(rp => rp.HandlerId);

        modelBuilder.Entity<Report>()
        .HasOne(rp => rp.Reason)
        .WithMany(rs => rs.Reports)
        .HasForeignKey(rp => rp.ReasonId);

        modelBuilder.Entity<Report>()
        .Property(rp => rp.CreateAt)
        .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<Report>()
        .Property(rp => rp.ReportStatus)
        .HasDefaultValue(false);

        // Comment
        modelBuilder.Entity<Comment>()
        .HasOne(c => c.User)
        .WithMany(u => u.Comments)
        .HasForeignKey(c => c.UserId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Comment>()
        .HasOne(c => c.Review)
        .WithMany(re => re.Comments)
        .HasForeignKey(c => c.ReviewId);

        modelBuilder.Entity<Comment>()
        .HasOne(c => c.Parent)
        .WithMany(c => c.Children)
        .HasForeignKey(c => c.ParentId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Comment>()
        .Property(c => c.CreateAt)
        .HasDefaultValueSql("GETUTCDATE()");

        // Vote
        modelBuilder.Entity<Vote>()
        .HasOne(v => v.User)
        .WithMany(u => u.Votes)
        .HasForeignKey(v => v.UserId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Vote>()
        .Property(v => v.CreateAt)
        .HasDefaultValueSql("GETUTCDATE()");

        // Save
        modelBuilder.Entity<Save>()
        .HasOne(s => s.User)
        .WithMany(u => u.Saves)
        .HasForeignKey(s => s.UserId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Save>()
        .HasOne(s => s.Review)
        .WithMany(re => re.Saves)
        .HasForeignKey(s => s.ReviewId);

        modelBuilder.Entity<Save>()
        .Property(s => s.CreateAt)
        .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<Save>()
        .HasIndex(s => new {s.ReviewId, s.UserId})
        .IsUnique();

        // Notification
        modelBuilder.Entity<Notification>()
        .HasOne(n => n.User)
        .WithMany(u => u.Notifications)
        .HasForeignKey(n => n.UserId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Notification>()
        .Property(n => n.CreateAt)
        .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<Notification>()
        .Property(n => n.IsRead)
        .HasDefaultValue(false);

        // History
        modelBuilder.Entity<History>()
        .HasOne(h => h.User)
        .WithMany(u => u.Histories)
        .HasForeignKey(h => h.UserId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<History>()
        .Property(h => h.CreateAt)
        .HasDefaultValueSql("GETUTCDATE()");
    }
}