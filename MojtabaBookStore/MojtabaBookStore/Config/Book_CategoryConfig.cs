using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MojtabaBookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojtabaBookStore.Config
{
    public class Book_CategoryConfig : IEntityTypeConfiguration<Book_Category>
    {
        public void Configure(EntityTypeBuilder<Book_Category> builder)
        {
            builder.HasKey(k => new { k.BookID, k.CategoryID});
            builder.HasOne(c => c.Book).WithMany(c => c.Book_Categories).HasForeignKey(c => c.BookID);
            builder.HasOne(c => c.Category).WithMany(c => c.Book_Categories).HasForeignKey(c => c.CategoryID);
        }
    }
}
