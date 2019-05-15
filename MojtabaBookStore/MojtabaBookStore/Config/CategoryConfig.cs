using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MojtabaBookStore.Models;

namespace MojtabaBookStore.Config
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
               new Category { CategoryID = 1, CategoryName = "هنر" },
               new Category { CategoryID = 2, CategoryName = "عمومی" },
               new Category { CategoryID = 3, CategoryName = "دانشگاهی" }
               );
        }
    }
}
