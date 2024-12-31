using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// اضافه کردن DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

// تنظیمات Cookie Authentication
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login"; // مسیر صفحه لاگین
        options.AccessDeniedPath = "/Account/AccessDenied"; // مسیر صفحه دسترسی غیرمجاز
    });

builder.Services.AddAuthorization();

// اضافه کردن MVC به سرویس‌ها
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middlewareهای مورد نیاز
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // اضافه کردن Authentication Middleware
app.UseAuthorization();

// تنظیمات برای استفاده از MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
