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
    public class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(p => p.FirstName).HasColumnName("FName").HasColumnType("nvarchar(20)");
            builder.Ignore(p => p.Age).Property(p => p.LastName).HasColumnName("LName").HasMaxLength(100);
            builder.HasOne(p => p.city1).WithMany(t => t.Customers1).HasForeignKey(p => p.CityID1);

            builder.HasOne(p => p.city2).WithMany(t => t.Customers2).HasForeignKey(p => p.CityID2).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
