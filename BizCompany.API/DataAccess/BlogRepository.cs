using BizCompany.API.Context;
using BizCompany.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace BizCompany.API.DataAccess
{
    public class BlogRepository(AppDbContext context)
    {
        public async Task<List<Blog>> GetBlogsWithDetailsAsync()
        {
            var values = await context.Blogs
                .Where(b => !b.IsDeleted && b.Writer != null && !b.Writer.IsDeleted
                            && b.Category != null && !b.Category.IsDeleted
                            && b.BlogTags != null && b.BlogTags.All(bt => !bt.IsDeleted)
                            && b.Comments != null && b.Comments.All(c => !c.IsDeleted))
                .Include(w => w.Writer)
                .Include(bc => bc.Category)
                .Include(bt => bt.BlogTags!)
                    .ThenInclude(t => t.Tag)
                .Include(c => c.Comments!)
                .AsNoTracking()
                .ToListAsync();

            return values;
        }

        public async Task<Blog?> GetBlogWithDetailsByIdAsync(int id)
        {
            var value = await context.Blogs
                .Where(b => b.Id == id && !b.IsDeleted && b.Writer != null && !b.Writer.IsDeleted
                            && b.Category != null && !b.Category.IsDeleted
                            && b.BlogTags != null && b.BlogTags.All(bt => !bt.IsDeleted)
                            && b.Comments != null && b.Comments.All(c => !c.IsDeleted))
                .Include(w => w.Writer)
                .Include(bc => bc.Category)
                .Include(bt => bt.BlogTags!)
                    .ThenInclude(t => t.Tag)
                .Include(c => c.Comments!)
                    .ThenInclude(w => w.Writer)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return value;
        }

        public async Task<bool> CreateBlogAsync(Blog blog)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                // While adding a new blog, ef core will atutomatically create blogtags entries if they are included in blog entity
                await context.Blogs.AddAsync(blog);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
        public async Task<bool> UpdateBlogAsync(Blog blog)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                // Get current blog with its BlogTags
                var existingBlog = await context.Blogs
                    .Include(b => b.BlogTags)
                    .FirstOrDefaultAsync(b => b.Id == blog.Id);

                if (existingBlog == null)
                    return false;

                // Update blog properties
                existingBlog.Title = blog.Title;
                existingBlog.Content = blog.Content;
                existingBlog.CoverImageUrl = blog.CoverImageUrl;
                existingBlog.ContentImageUrl = blog.ContentImageUrl;
                existingBlog.WriterId = blog.WriterId;
                existingBlog.CategoryId = blog.CategoryId;

                // Update BlogTags
                var incomingTagIds = blog.BlogTags?.Select(bt => bt.TagId).ToList() ?? new List<int>();
                var existingBlogTags = existingBlog.BlogTags?.Where(bt => !bt.IsDeleted).ToList() ?? new List<BlogTag>();

                // Soft delete removed BlogTags
                foreach (var existingTag in existingBlogTags)
                {
                    if (!incomingTagIds.Contains(existingTag.TagId))
                    {
                        existingTag.IsDeleted = true;
                    }
                }

                // Yeni eklenen BlogTag'leri ekle
                var existingTagIds = existingBlogTags.Select(bt => bt.TagId).ToList();
                foreach (var incomingTagId in incomingTagIds)
                {
                    if (!existingTagIds.Contains(incomingTagId))
                    {
                        existingBlog.BlogTags!.Add(new BlogTag
                        {
                            BlogId = blog.Id,
                            TagId = incomingTagId,
                            IsDeleted = false
                        });
                    }
                }

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

    }
}
