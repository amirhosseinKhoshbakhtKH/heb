using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlexIn.Models;

[Authorize]
public class ProductsController : Controller
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    // نمایش لیست محصولات
    public async Task<IActionResult> Index()
    {
        // بررسی وجود داده در جدول
        var hasProducts = await _context.Products.AnyAsync();

        if (!hasProducts)
        {
            // هدایت به صفحه "خالی بودن محصولات"
            return RedirectToAction("Empty");
        }

        // اگر داده وجود داشت، لیست محصولات را بفرست
        var products = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Menu)
            .ToListAsync();

        return View(products);
    }



    // حذف محصول
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // نمایش فرم ساخت محصول جدید
    public IActionResult Create()
    {
        ViewBag.Features = _context.Features
            .Include(f => f.Options)
            .Where(f => f.BusinessId == 1) // شناسه بیزنس فعلی
            .ToList();

        ViewBag.Categories = _context.Categories.ToList(); // دسته‌بندی‌ها
        return View();
    }

    // ساخت محصول جدید
    [HttpPost]
    public async Task<IActionResult> Create(Product product, List<int> featureIds, List<int> optionIds)
    {
        if (ModelState.IsValid)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // ذخیره ویژگی‌ها و گزینه‌ها برای محصول
            for (int i = 0; i < featureIds.Count; i++)
            {
                _context.ProductFeatures.Add(new ProductFeature
                {
                    ProductId = product.Id,
                    FeatureId = featureIds[i],
                    OptionId = optionIds[i]
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // اگر مدل نامعتبر بود، دوباره داده‌ها را به View بفرست
        ViewBag.Features = _context.Features.Include(f => f.Options).ToList();
        ViewBag.Categories = _context.Categories.ToList();
        return View(product);
    }


    // مدیریت ویژگی‌ها
    public IActionResult Features()
    {
        return View();
    }

    // اضافه کردن ویژگی جدید
    [HttpPost]
    public async Task<IActionResult> AddFeature(string featureName, List<string> options)
    {
        var feature = new Feature
        {
            Name = featureName,
            BusinessId = 1 // ID بیزنس فعلی را جایگزین کن
        };
        _context.Features.Add(feature);
        await _context.SaveChangesAsync();

        foreach (var option in options)
        {
            var featureOption = new FeatureOption
            {
                Value = option,
                FeatureId = feature.Id
            };
            _context.FeatureOptions.Add(featureOption);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Features));
    }

    public IActionResult Empty()
    {
        return View();
    }

    // اضافه کردن ویژگی به محصول
    [HttpPost]
    public async Task<IActionResult> CreateFeature(string featureName, List<string> options)
    {
        if (string.IsNullOrEmpty(featureName))
        {
            ModelState.AddModelError("FeatureName", "نام ویژگی نمی‌تواند خالی باشد.");
            return RedirectToAction("Empty");
        }

        var feature = new Feature
        {
            Name = featureName,
            BusinessId = 1 // شناسه بیزنس فعلی (برای تست ثابت است)
        };

        _context.Features.Add(feature);
        await _context.SaveChangesAsync();

        // اضافه کردن گزینه‌ها به ویژگی
        foreach (var option in options)
        {
            if (!string.IsNullOrWhiteSpace(option))
            {
                _context.FeatureOptions.Add(new FeatureOption
                {
                    FeatureId = feature.Id,
                    Value = option
                });
            }
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Empty");
    }


    [HttpPost]
    public async Task<IActionResult> CreateCategory(string categoryName)
    {
        if (string.IsNullOrEmpty(categoryName))
        {
            ModelState.AddModelError("CategoryName", "نام دسته‌بندی نمی‌تواند خالی باشد.");
            return RedirectToAction("Empty");
        }

        var category = new Category
        {
            Name = categoryName
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

}
