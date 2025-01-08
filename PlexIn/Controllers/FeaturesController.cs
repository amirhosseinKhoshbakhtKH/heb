using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlexIn.Models;

[Authorize]
public class FeaturesController : Controller
{
    private readonly AppDbContext _context;

    public FeaturesController(AppDbContext context)
    {
        _context = context;
    }

    // نمایش لیست ویژگی‌ها
    public async Task<IActionResult> Index()
    {
        var businessIdClaim = User.Claims.FirstOrDefault(c => c.Type == "BusinessId");
        if (businessIdClaim == null)
        {
            TempData["Error"] = "اطلاعات بیزنس لاگین‌شده یافت نشد.";
            return RedirectToAction("Logout", "Account");
        }

        int businessId = int.Parse(businessIdClaim.Value);

        var features = await _context.Features
            .Include(f => f.Options) // بارگذاری مقادیر ویژگی
            .Where(f => f.BusinessId == businessId)
            .ToListAsync();

        return View(features); // ارسال لیست به ویو
    }

    // متد GET برای نمایش فرم
    [HttpGet]
    public IActionResult Create()
    {
        return View(); // نمایش فرم ساخت ویژگی
    }

    // ساخت ویژگی جدید
    [HttpPost]
    public async Task<IActionResult> Create(string featureName, List<FeatureOption> Options)
    {
        if (string.IsNullOrEmpty(featureName) || Options == null || !Options.Any())
        {
            TempData["Error"] = "نام ویژگی و حداقل یک مقدار باید وارد شود.";
            return RedirectToAction(nameof(Create));
        }

        var businessIdClaim = User.Claims.FirstOrDefault(c => c.Type == "BusinessId");
        if (businessIdClaim == null)
        {
            TempData["Error"] = "اطلاعات بیزنس لاگین‌شده یافت نشد.";
            return RedirectToAction("Logout", "Account");
        }

        int businessId = int.Parse(businessIdClaim.Value);

        var feature = new Feature
        {
            Name = featureName,
            BusinessId = businessId,
            Options = Options
        };

        _context.Features.Add(feature);
        await _context.SaveChangesAsync();

        TempData["Success"] = "ویژگی جدید با موفقیت ایجاد شد.";
        return RedirectToAction(nameof(Index));
    }


    // حذف ویژگی
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var feature = await _context.Features
            .Include(f => f.Options)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (feature == null)
        {
            TempData["Error"] = "ویژگی یافت نشد.";
            return RedirectToAction(nameof(Index));
        }

        _context.FeatureOptions.RemoveRange(feature.Options);
        _context.Features.Remove(feature);
        await _context.SaveChangesAsync();

        TempData["Success"] = "ویژگی با موفقیت حذف شد.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> GetOptions(int featureId)
    {
        var options = await _context.FeatureOptions
            .Where(o => o.FeatureId == featureId)
            .Select(o => new { o.Id, o.Value })
            .ToListAsync();

        return Json(options);
    }


}
