using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MojtabaBookStore.Models;
using System;

namespace MojtabaBookStore.Config
{
    public class Order_BookConfig : IEntityTypeConfiguration<Order_Book>
    {
        public void Configure(EntityTypeBuilder<Order_Book> builder)
        {
            builder.HasKey(t => new { t.BookID, t.OrderID });

            builder.HasOne(p => p.Book).WithMany(t => t.Order_Books).HasForeignKey(f => f.BookID);

            builder.HasOne(p => p.Order).WithMany(t => t.Order_Books).HasForeignKey(f => f.OrderID);
        }
    }
}
