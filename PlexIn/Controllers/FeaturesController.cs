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
        var features = await _context.Features
            .Include(f => f.Options)
            .ToListAsync();
        return View(features);
    }

    // ساخت ویژگی جدید
    [HttpPost]
    public async Task<IActionResult> Create(string featureName, List<string> options)
    {
        if (string.IsNullOrEmpty(featureName))
        {
            TempData["Error"] = "نام ویژگی نمی‌تواند خالی باشد.";
            return RedirectToAction(nameof(Index));
        }

        var feature = new Feature { Name = featureName, BusinessId = 1 };
        _context.Features.Add(feature);
        await _context.SaveChangesAsync();

        foreach (var option in options.Where(o => !string.IsNullOrWhiteSpace(o)))
        {
            _context.FeatureOptions.Add(new FeatureOption { FeatureId = feature.Id, Value = option });
        }

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
}
