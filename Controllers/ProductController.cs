using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FirstAspNetApp.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
