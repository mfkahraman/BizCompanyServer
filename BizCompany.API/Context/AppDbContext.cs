using BizCompany.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BizCompany.API.Context
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Writer> Writers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Message> Messages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // BlogTag için composite key - ZORUNLU
            // Bunu yapmayınca migration oluştururken primary key hatası veriyor
            modelBuilder.Entity<BlogTag>()
                .HasKey(bt => new { bt.BlogId, bt.TagId });

            // Soft delete filters
            modelBuilder.Entity<Blog>().HasQueryFilter(b => !b.IsDeleted);
            modelBuilder.Entity<BlogCategory>().HasQueryFilter(bc => !bc.IsDeleted);
            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Tag>().HasQueryFilter(t => !t.IsDeleted);
            modelBuilder.Entity<Comment>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Writer>().HasQueryFilter(w => !w.IsDeleted);
            modelBuilder.Entity<Testimonial>().HasQueryFilter(t => !t.IsDeleted);
            modelBuilder.Entity<Message>().HasQueryFilter(m => !m.IsDeleted);
        }
    }
}
