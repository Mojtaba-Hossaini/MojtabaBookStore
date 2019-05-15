using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MojtabaBookStore.Models;

namespace MojtabaBookStore.Config
{
    public class ProvinceConfig : IEntityTypeConfiguration<Province>
    {
        public void Configure(EntityTypeBuilder<Province> builder)
        {
            builder.Property(p => p.ProvinceID).ValueGeneratedNever();
        }
    }
}
