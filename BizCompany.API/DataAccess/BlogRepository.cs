using BizCompany.API.Context;
using BizCompany.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;

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
    }
}
