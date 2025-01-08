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
        var businessIdClaim = User.Claims.FirstOrDefault(c => c.Type == "BusinessId");
        if (businessIdClaim == null)
        {
            TempData["Error"] = "اطلاعات بیزنس لاگین‌شده یافت نشد.";
            return RedirectToAction("Logout", "Account");
        }

        int businessId = int.Parse(businessIdClaim.Value);

        var categories = await _context.Categories
            .Include(c => c.Business)
            .Where(c => c.BusinessId == businessId)
            .ToListAsync();

        return View(categories);
    }



    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(string categoryName)
    {
        if (string.IsNullOrEmpty(categoryName))
        {
            TempData["Error"] = "نام دسته‌بندی نمی‌تواند خالی باشد.";
            return RedirectToAction(nameof(Create));
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        // دریافت BusinessId کاربر لاگین‌شده
        var businessIdClaim = User.Claims.FirstOrDefault(c => c.Type == "BusinessId");
        if (businessIdClaim == null)
        {
            TempData["Error"] = "اطلاعات بیزنس لاگین‌شده یافت نشد.";
            return RedirectToAction(nameof(Index));
        }

        int businessId = int.Parse(businessIdClaim.Value);

        // پیدا کردن دسته‌بندی مربوطه
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.BusinessId == businessId);

        if (category == null)
        {
            TempData["Error"] = "دسته‌بندی یافت نشد یا به شما تعلق ندارد.";
            return RedirectToAction(nameof(Index));
        }

        // حذف دسته‌بندی
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        TempData["Success"] = "دسته‌بندی با موفقیت حذف شد.";
        return RedirectToAction(nameof(Index));
    }


}
