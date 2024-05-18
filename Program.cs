using AzureImage.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlite("DataSource=mydatabase.db");
    SQLitePCL.Batteries.Init();
});
builder.Services.AddMvc();
builder.Services.AddControllersWithViews();
var app = builder.Build();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}");
app.UseStaticFiles();
app.Run();
