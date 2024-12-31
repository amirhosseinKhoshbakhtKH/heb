using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlexIn.Models;

[Authorize]
public class CategoriesController : Controller
{
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }

    // نمایش لیست دسته‌بندی‌ها
    public async Task<IActionResult> Index()
    {
        // دریافت BusinessId از کاربر لاگین‌شده
        var businessIdClaim = User.Claims.FirstOrDefault(c => c.Type == "BusinessId");
        if (businessIdClaim == null)
        {
            TempData["Error"] = "اطلاعات بیزنس لاگین‌شده یافت نشد.";
            return RedirectToAction("Logout", "Account"); // به صفحه خروج هدایت شود
        }

        int businessId = int.Parse(businessIdClaim.Value);

        // فیلتر دسته‌بندی‌ها برای این بیزنس
        var categories = await _context.Categories
            .Where(c => c.BusinessId == businessId)
            .ToListAsync();

        return View(categories);
    }


    public IActionResult Create()
    {
        return View();
    }


    // ساخت دسته‌بندی جدید
    [HttpPost]
    public async Task<IActionResult> Create(string categoryName)
    {
        if (string.IsNullOrEmpty(categoryName))
        {
            TempData["Error"] = "نام دسته‌بندی نمی‌تواند خالی باشد.";
            return RedirectToAction(nameof(Index));
        }

        // دریافت businessId از کاربر لاگین‌شده
        var businessIdClaim = User.Claims.FirstOrDefault(c => c.Type == "BusinessId");
        if (businessIdClaim == null)
        {
            TempData["Error"] = "اطلاعات بیزنس لاگین‌شده یافت نشد.";
            return RedirectToAction(nameof(Index));
        }

        int businessId = int.Parse(businessIdClaim.Value);

        // ساخت دسته‌بندی جدید
        var category = new Category
        {
            Name = categoryName,
            BusinessId = businessId
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        TempData["Success"] = "دسته‌بندی جدید با موفقیت ایجاد شد.";
        return RedirectToAction(nameof(Index));
    }


    // حذف دسته‌بندی
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            TempData["Error"] = "دسته‌بندی یافت نشد.";
            return RedirectToAction(nameof(Index));
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        TempData["Success"] = "دسته‌بندی با موفقیت حذف شد.";
        return RedirectToAction(nameof(Index));
    }
}
