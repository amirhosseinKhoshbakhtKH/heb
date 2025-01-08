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

    private async Task PopulateViewBags(int businessId)
    {
        ViewBag.Categories = await _context.Categories
            .Where(c => c.BusinessId == businessId)
            .ToListAsync();

        ViewBag.Features = await _context.Features
            .Where(f => f.BusinessId == businessId)
            .ToListAsync();
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var businessIdClaim = User.Claims.FirstOrDefault(c => c.Type == "BusinessId");
        if (businessIdClaim == null)
        {
            TempData["Error"] = "اطلاعات بیزنس لاگین‌شده یافت نشد.";
            return RedirectToAction("Logout", "Account");
        }
        int businessId = int.Parse(businessIdClaim.Value);

        await PopulateViewBags(businessId);

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product, List<ProductFeature> Features, List<IFormFile> Images)
    {
        Console.WriteLine($"Product: {product.Name}, {product.Price}, {product.CategoryId}");
        if (Features != null)
        {
            foreach (var feature in Features)
            {
                Console.WriteLine($"FeatureId: {feature.FeatureId}, FeatureOptionId: {feature.FeatureOptionId}");
            }
        }

        if (Images != null)
        {
            foreach (var image in Images)
            {
                Console.WriteLine($"Image: {image.FileName}");
            }
        }
        var businessIdClaim = User.Claims.FirstOrDefault(c => c.Type == "BusinessId");
        if (businessIdClaim == null)
        {
            TempData["Error"] = "اطلاعات بیزنس لاگین‌شده یافت نشد.";
            return RedirectToAction("Logout", "Account");
        }
        int businessId = int.Parse(businessIdClaim.Value);
        if (!ModelState.IsValid)
        {
            var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            // یا در یک breakpoint قرار دهید و errorMessages را بررسی کنید
            foreach (var err in errorMessages)
            {
                Console.WriteLine(err); // یا هر لاگ دیگری
            }

            TempData["Error"] = "اطلاعات وارد شده کامل نیست.";
            await PopulateViewBags(businessId);
            return View(product);
        }

        // ذخیره محصول
        _context.Products.Add(product);
        await _context.SaveChangesAsync();


        // ذخیره ویژگی‌های محصول
        if (Features != null)
        {
            foreach (var feature in Features)
            {
                if (feature.FeatureId > 0 && feature.FeatureOptionId > 0)
                {
                    feature.ProductId = product.Id;
                    _context.ProductFeatures.Add(feature);
                }
            }
        }

        // ذخیره تصاویر محصول
        if (Images != null && Images.Any())
        {
            foreach (var image in Images)
            {
                if (image.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot/images/products", image.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    _context.ProductImages.Add(new ProductImage
                    {
                        ProductId = product.Id,
                        Url = $"/images/products/{image.FileName}"
                    });
                }
            }
        }

        await _context.SaveChangesAsync();
        TempData["Success"] = "محصول با موفقیت ایجاد شد.";
        return RedirectToAction("Index");
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
