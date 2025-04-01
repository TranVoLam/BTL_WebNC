using BTL_WebNC.Data;
using BTL_WebNC.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScopedRepository();
builder.Services.AddSessionCookie();
builder.Services.AddDbContext<WebNCDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
var app = builder.Build();

app.UseSession();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
