using Microsoft.EntityFrameworkCore;
using MySharedModels;

namespace MyApiProject
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet ها برای هر مدل
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<FeatureOption> FeatureOptions { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Campaign> Campaigns { get; set; } // اضافه کردن مدل Campaign
        public DbSet<Reward> Rewards { get; set; } // اضافه کردن مدل Reward
        public DbSet<Customer> Customers { get; set; } // اضافه کردن مدل Customer
        public DbSet<StarTransaction> StarTransactions { get; set; } // اضافه کردن مدل StarTransaction
        public DbSet<DiscountCode> DiscountCodes { get; set; } // اضافه کردن مدل DiscountCode
        public DbSet<Comment> Comments { get; set; } // اضافه کردن مدل Comment
        public DbSet<CommentReply> CommentReplies { get; set; } // اضافه کردن مدل CommentReply

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // تعریف ایندکس‌ها و ارتباطات
            modelBuilder.Entity<Business>()
                .HasIndex(b => b.Username)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Username)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasIndex(p => new { p.Name, p.CategoryId }) // ایندکس ترکیبی
                .IsUnique(false);

            modelBuilder.Entity<DiscountCode>()
                .HasIndex(d => d.Code)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
