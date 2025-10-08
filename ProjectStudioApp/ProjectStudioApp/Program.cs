using Microsoft.EntityFrameworkCore;
using ProjectStudioApp.Datafile;

var builder = WebApplication.CreateBuilder(args);

// Add session services
builder.Services.AddSession();

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ZooliranteDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ZooliranteDbContext")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use session middleware
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();