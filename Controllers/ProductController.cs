using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FirstAspNetApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAspNetApp.Controllers;

public class ProductController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public ProductController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        using (Models.OrderDBContext context = new Models.OrderDBContext())
        {
            ViewData["Items"] = context.Items.OrderBy(i => i.Category).ToList();
        }
        return View();
    }

    public IActionResult TatCa()
    {
        using (Models.OrderDBContext context = new Models.OrderDBContext())
        {
            ViewData["Items"] = context.Items.OrderBy(i => i.ItemId).ToList();
        }
        return View();
    }

    public IActionResult DoAnKem()
    {
        string water = "Đồ Ăn Kèm";
        using (Models.OrderDBContext context = new Models.OrderDBContext())
        {
            ViewData["Items"] = context.Items.OrderBy(i => i.ItemId).Where(i => i.Category == water).ToList();
        }
        return View();
    }

    public IActionResult Thit()
    {
        string water = "Thịt";
        using (Models.OrderDBContext context = new Models.OrderDBContext())
        {
            ViewData["Items"] = context.Items.OrderBy(i => i.ItemId).Where(i => i.Category == water).ToList();
        }
        return View();
    }

    public IActionResult Rau()
    {
        string water = "Rau";
        using (Models.OrderDBContext context = new Models.OrderDBContext())
        {
            ViewData["Items"] = context.Items.OrderBy(i => i.ItemId).Where(i => i.Category == water).ToList();
        }
        return View();
    }

    public IActionResult Canh()
    {
        string water = "Canh";
        using (Models.OrderDBContext context = new Models.OrderDBContext())
        {
            ViewData["Items"] = context.Items.OrderBy(i => i.ItemId).Where(i => i.Category == water).ToList();
        }
        return View();
    }

    public IActionResult Nuoc()
    {
        string water = "Nước";
        using (Models.OrderDBContext context = new Models.OrderDBContext())
        {
            ViewData["Items"] = context.Items.OrderBy(i => i.ItemId).Where(i => i.Category == water).ToList();
        }
        return View();
    }

    public IActionResult ProductDetail(int? id)
    {
        using (Models.OrderDBContext context = new Models.OrderDBContext())
        {
            ViewData["Feedbacks"] = context.Feedbacks.OrderBy(a => a.FeedbackId).ToList();
            ViewData["Item"] = context.Items.Single(i => i.ItemId == id);
            if (ViewData["Item"] == null)
            {
                TempData["msg"] = "Can't find Item with id = " + id;
            }
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
