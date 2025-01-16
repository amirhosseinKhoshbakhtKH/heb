using Microsoft.EntityFrameworkCore;
using PlexIn.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Business> Businesses { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Feature> Features { get; set; }
    public DbSet<FeatureOption> FeatureOptions { get; set; }
    public DbSet<ProductFeature> ProductFeatures { get; set; }
    public DbSet<Campaign> Campaigns { get; set; } // اضافه شد
    public DbSet<Reward> Rewards { get; set; } // اضافه شد
    public DbSet<Customer> Customers { get; set; } // اضافه شد
    public DbSet<StarTransaction> StarTransactions { get; set; } // اضافه شد
    public DbSet<DiscountCode> DiscountCodes { get; set; } // اضافه شد
    public DbSet<Comment> Comments { get; set; } // اضافه شد
    public DbSet<CommentReply> CommentReplies { get; set; } // اضافه شد

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Feature و FeatureOption
        modelBuilder.Entity<Feature>()
            .HasMany(f => f.Options)
            .WithOne(o => o.Feature)
            .HasForeignKey(o => o.FeatureId)
            .OnDelete(DeleteBehavior.Cascade);

        // Business و Feature
        modelBuilder.Entity<Feature>()
            .HasOne(f => f.Business)
            .WithMany(b => b.Features)
            .HasForeignKey(f => f.BusinessId)
            .OnDelete(DeleteBehavior.Cascade);

        // Business و Menu
        modelBuilder.Entity<Menu>()
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

        // ProductFeature
        modelBuilder.Entity<ProductFeature>()
            .HasOne(pf => pf.Product)
            .WithMany(p => p.ProductFeatures)
            .HasForeignKey(pf => pf.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProductFeature>()
            .HasOne(pf => pf.Feature)
            .WithMany()
            .HasForeignKey(pf => pf.FeatureId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProductFeature>()
            .HasOne(pf => pf.FeatureOption)
            .WithMany()
            .HasForeignKey(pf => pf.FeatureOptionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Campaign و Reward
        modelBuilder.Entity<Reward>()
            .HasOne(r => r.Campaign)
            .WithMany()
            .HasForeignKey(r => r.CampaignId)
            .OnDelete(DeleteBehavior.Cascade);

        // Customer و StarTransaction
        modelBuilder.Entity<StarTransaction>()
            .HasOne(st => st.Customer)
            .WithMany(c => c.StarTransactions)
            .HasForeignKey(st => st.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StarTransaction>()
            .HasOne(st => st.Business)
            .WithMany()
            .HasForeignKey(st => st.BusinessId)
            .OnDelete(DeleteBehavior.Restrict);

        // DiscountCode
        modelBuilder.Entity<DiscountCode>()
            .HasOne(dc => dc.Customer)
            .WithMany(c => c.DiscountCodes)
            .HasForeignKey(dc => dc.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DiscountCode>()
            .HasOne(dc => dc.Business)
            .WithMany()
            .HasForeignKey(dc => dc.BusinessId)
            .OnDelete(DeleteBehavior.Cascade);

        // Comment و CommentReply
        modelBuilder.Entity<CommentReply>()
            .HasOne(cr => cr.Comment)
            .WithMany(c => c.Replies)
            .HasForeignKey(cr => cr.CommentId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}
