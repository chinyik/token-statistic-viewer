using Microsoft.EntityFrameworkCore;
using TokenScanner.Models;
using TokenScanner.Services.Export;
using TokenScanner.Services.TokenPrice;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TokenContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("ConnStr")));

builder.Services.AddSingleton<IExportService, TokenExportService>();
builder.Services.AddSingleton<ITokenPriceService, TokenPriceService>();

builder.Services.AddHostedService<TokenPriceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
