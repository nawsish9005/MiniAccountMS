using MiniAccountMS.Data;
using MiniAccountMS.Models;
using MiniAccountMS.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ApplicationDbContext>();

builder.Services.AddScoped<PermissionService>();
builder.Services.AddScoped<VoucharService>();

// ✅ Add Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// ✅ Enable Middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
