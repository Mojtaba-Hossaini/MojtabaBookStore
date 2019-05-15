using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MojtabaBookStore.Models;

namespace MojtabaBookStore.Config
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(b => b.Title).IsRequired();
            builder.HasKey(b => b.BookID);
            builder.Property(b => b.Image).HasColumnType("image");
            builder.ToTable("BookInfo");

            builder
               .HasOne(p => p.Discount) 
               .WithOne(t => t.Book)
               .HasForeignKey<Discount>(p => p.BookID);

            builder
               .HasOne(p => p.SubCategory)
               .WithMany(t => t.Books)
               .HasForeignKey(f => f.SCategoryID);
        }
    }
}
