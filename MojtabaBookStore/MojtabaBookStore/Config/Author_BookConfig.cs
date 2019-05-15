using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MojtabaBookStore.Models;

namespace MojtabaBookStore.Config
{
    public class Author_BookConfig : IEntityTypeConfiguration<Author_Book>
    {
        public void Configure(EntityTypeBuilder<Author_Book> builder)
        {
            builder.HasKey(t => new { t.BookID, t.AuthorID });
            builder
              .HasOne(p => p.Book)
              .WithMany(t => t.Author_Books)
              .HasForeignKey(f => f.BookID);

            builder
               .HasOne(p => p.Author)
               .WithMany(t => t.Author_Books)
               .HasForeignKey(f => f.AuthorID);
        }
    }
}
