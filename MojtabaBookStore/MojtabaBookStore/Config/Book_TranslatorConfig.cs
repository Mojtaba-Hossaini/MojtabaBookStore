using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MojtabaBookStore.Models;
using System;

namespace MojtabaBookStore.Config
{
    public class Book_TranslatorConfig : IEntityTypeConfiguration<Book_Translator>
    {
        public void Configure(EntityTypeBuilder<Book_Translator> builder)
        {
            builder.HasKey(k => new { k.BookID, k.TranslatroID });
            builder.HasOne(b => b.Book).WithMany(b => b.Book_Translators).HasForeignKey(f => f.BookID);
            builder.HasOne(b => b.Translator).WithMany(b => b.Book_Translators).HasForeignKey(f => f.TranslatroID);
        }
    }
}
