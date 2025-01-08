using Microsoft.EntityFrameworkCore;
using PlexIn.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Business> Businesses { get; set; }
    public DbSet<Menu> Menus { get; set; } // اصلاح شد
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Feature> Features { get; set; } // اصلاح شد
    public DbSet<FeatureOption> FeatureOptions { get; set; }
    public DbSet<ProductFeature> ProductFeatures { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Feature و FeatureOption
        modelBuilder.Entity<Feature>() // اصلاح شد
            .HasMany(f => f.Options)
            .WithOne(o => o.Feature)
            .HasForeignKey(o => o.FeatureId)
            .OnDelete(DeleteBehavior.Cascade);

        // Business و Feature
        modelBuilder.Entity<Feature>() // اصلاح شد
            .HasOne(f => f.Business)
            .WithMany(b => b.Features)
            .HasForeignKey(f => f.BusinessId)
            .OnDelete(DeleteBehavior.Cascade);

        // Business و Menu
        modelBuilder.Entity<Menu>() // اصلاح شد
            .HasOne(m => m.Business)
            .WithMany(b => b.Menus)
            .HasForeignKey(m => m.BusinessId)
            .OnDelete(DeleteBehavior.Cascade);

        // Product و ProductImage
        modelBuilder.Entity<ProductImage>()
            .HasOne(pi => pi.Product)
            .WithMany(p => p.ProductImages)
            .HasForeignKey(pi => pi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProductFeature>()
        .HasOne(pf => pf.Product)
        .WithMany(p => p.ProductFeatures)
        .HasForeignKey(pf => pf.ProductId)
        .OnDelete(DeleteBehavior.Cascade); // حذف Product باعث حذف ProductFeature شود

        modelBuilder.Entity<ProductFeature>()
            .HasOne(pf => pf.Feature)
            .WithMany()
            .HasForeignKey(pf => pf.FeatureId)
            .OnDelete(DeleteBehavior.Cascade); // حذف Feature باعث حذف ProductFeature شود

        modelBuilder.Entity<ProductFeature>()
            .HasOne(pf => pf.FeatureOption)
            .WithMany()
            .HasForeignKey(pf => pf.FeatureOptionId)
            .OnDelete(DeleteBehavior.Restrict); // جلوگیری از حذف FeatureOption اگر در ProductFeature استفاده شده است


        base.OnModelCreating(modelBuilder);
    }
}
