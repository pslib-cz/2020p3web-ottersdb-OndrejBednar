using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OttersDatabase.Models;

[assembly: HostingStartup(typeof(OttersDatabase.Areas.Identity.IdentityHostingStartup))]
namespace OttersDatabase.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<OtterDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("OtterDbContextConnection")));
            });
        }
    }
}