using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FirstAspNetApp.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using Newtonsoft.Json;

namespace FirstAspNetApp.Controllers;

public class ProductController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly OrderDBContext _context;

    public ProductController(ILogger<HomeController> logger, OrderDBContext contexted)
    {
        _logger = logger;
        _context = contexted;
    }

    public IActionResult Index()
    {
        ViewData["Items"] = _context.Items.OrderBy(i => i.Category).ToList();
        return View();
    }

    public IActionResult TatCa(int? page = 1)
    {
        if (page != null && page < 1)
        {
            page = 1;
        }
        var pageSize = 12;
        var item = _context.Items.OrderBy(i => i.ItemId).ToPagedList(page ?? 1, pageSize);
        // ViewData["Items"] = context.Items.OrderBy(i => i.ItemId).ToList(); //

        return View(item);
    }

    public IActionResult DoAnKem()
    {
        string water = "Đồ Ăn Kèm";
        ViewData["Items"] = _context.Items.OrderBy(i => i.ItemId).Where(i => i.Category == water).ToList();
        return View();
    }

    public IActionResult Thit()
    {
        string water = "Thịt";
        ViewData["Items"] = _context.Items.OrderBy(i => i.ItemId).Where(i => i.Category == water).ToList();
        return View();
    }

    public IActionResult Rau()
    {
        string water = "Rau";
        ViewData["Items"] = _context.Items.OrderBy(i => i.ItemId).Where(i => i.Category == water).ToList();
        return View();
    }

    public IActionResult Canh()
    {
        string water = "Canh";
        ViewData["Items"] = _context.Items.OrderBy(i => i.ItemId).Where(i => i.Category == water).ToList();
        return View();
    }

    public IActionResult Nuoc()
    {
        string water = "Nước";
        ViewData["Items"] = _context.Items.OrderBy(i => i.ItemId).Where(i => i.Category == water).ToList();
        return View();
    }

    public IActionResult ProductDetail(int? id)
    {
        ViewData["Feedbacks"] = _context.Feedbacks.OrderBy(i => i.FeedbackId).ToList();
        ViewData["Items"] = _context.Items.Single(i => i.ItemId == id);
        if (ViewData["Items"] == null)
        {
            TempData["msg"] = "Can't find Item with id = " + id;
        }
        return View();
    }

    [HttpPost]
    // [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FeedbackName,FeedbackStory")] Feedback feedback)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TatCa));
            }
        }
        catch (DbUpdateException /* ex */)
        {
            //Log the error (uncomment ex variable name and write a log.
            ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists " +
                "see your system administrator.");
        }
        return View(feedback);
    }

    [HttpGet]
    public async Task<IActionResult> Search(string searchString)
    {
        var movies = from m in _context.Items
                     select m;

        if (!String.IsNullOrEmpty(searchString))
        {
            movies = movies.Where(s => s.ItemName.Contains(searchString));
        }
        ViewBag.Message = searchString;
        return View(await movies.ToListAsync());
    }

    /*     Shopping Cart     */

    // Hiện thị giỏ hàng
    public IActionResult Cart()
    {
        return View(GetCartItems());
    }

    public const string CARTKEY = "cart";

    // Lấy cart từ Session (danh sách CartItem)
    List<CartItem> GetCartItems()
    {
        var session = HttpContext.Session;
        string jsoncart = session.GetString(CARTKEY);
        if (jsoncart != null)
        {
            return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
        }
        return new List<CartItem>();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
