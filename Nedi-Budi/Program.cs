using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nedi_Budi.Context;
using Nedi_Budi.Helper;
using Nedi_Budi.Models;
using System.Threading.Tasks;

namespace Nedi_Budi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<DbContextRole>();
            builder.Services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });
            builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
            {

            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            var app = builder.Build();
            var scope = app.Services.CreateScope();
            var contextRole = scope.ServiceProvider.GetRequiredService<DbContextRole>();
            await contextRole.RoleDatabase();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Dasboard}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
