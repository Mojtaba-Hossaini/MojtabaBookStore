using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MojtabaBookStore.Areas.Identity.Data;
using MojtabaBookStore.Models;

[assembly: HostingStartup(typeof(MojtabaBookStore.Areas.Identity.IdentityHostingStartup))]
namespace MojtabaBookStore.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<MojtabaIdentityContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("MojtabaIdentityContextConnection")));

                //services.AddDefaultIdentity<MojtabaBookStoreUser>()
                //    .AddEntityFrameworkStores<MojtabaIdentityContext>();

                services.AddIdentity<ApplicationUser, ApplicationRole>()
                   .AddDefaultUI()
                   .AddEntityFrameworkStores<MojtabaIdentityContext>()
                   .AddErrorDescriber<ApplicationIdentityErrorDescriber>()
                   .AddDefaultTokenProviders();

                services.Configure<IdentityOptions>(options =>
                {
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 8;
                });
            });
        }
    }
}