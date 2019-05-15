using Microsoft.EntityFrameworkCore;
using MojtabaBookStore.Config;

namespace MojtabaBookStore.Models
{
    public class BookStoreDb : DbContext
    {
        public BookStoreDb(DbContextOptions<BookStoreDb> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Author_BookConfig());
            modelBuilder.ApplyConfiguration(new BookConfig());
            modelBuilder.ApplyConfiguration(new DiscountConfig());
            modelBuilder.ApplyConfiguration(new CustomerConfig());
            modelBuilder.ApplyConfiguration(new CityConfig());
            modelBuilder.ApplyConfiguration(new ProvinceConfig());
            modelBuilder.ApplyConfiguration(new Order_BookConfig());
            modelBuilder.ApplyConfiguration(new CategoryConfig());
        }
        DbSet<Book> Books { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<SubCategory> SubCategories { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<Author> Authors { get; set; }
        DbSet<City> Cities { get; set; }
        DbSet<Province> Provices { get; set; }
        DbSet<Author_Book> Author_Books { get; set; }
        DbSet<Order_Book> Order_Books { get; set; }
        DbSet<Language> Languages { get; set; }
        DbSet<Discount> Discounts { get; set; }
        DbSet<OrderStatus> OrderStatuses { get; set; }
        DbSet<Customer> Customers { get; set; }
    }
}
