using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FirstAspNetApp.Models;

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
            ViewData["Items"] = context.Items.OrderBy(i => i.UnitPrice).ToList();
        }
        return View();
    }

    public IActionResult TatCa()
    {
        using (Models.OrderDBContext context = new Models.OrderDBContext())
        {
            ViewData["Items"] = context.Items.OrderBy(i => i.UnitPrice).ToList();
        }
        return View();
    }

    public IActionResult DoAnKem()
    {
        using (Models.OrderDBContext context = new Models.OrderDBContext())
        {
            ViewData["Items"] = context.Items.OrderBy(i => i.UnitPrice).ToList();
        }
        return View();
    }

    public IActionResult Thit()
    {
        using (Models.OrderDBContext context = new Models.OrderDBContext())
        {
            ViewData["Items"] = context.Items.OrderBy(i => i.UnitPrice).ToList();
        }
        return View();
    }

    public IActionResult Rau()
    {
        using (Models.OrderDBContext context = new Models.OrderDBContext())
        {
            ViewData["Items"] = context.Items.OrderBy(i => i.UnitPrice).ToList();
        }
        return View();
    }

    public IActionResult Canh()
    {
        using (Models.OrderDBContext context = new Models.OrderDBContext())
        {
            ViewData["Items"] = context.Items.OrderBy(i => i.UnitPrice).ToList();
        }
        return View();
    }

    public IActionResult Nuoc()
    {
        using (Models.OrderDBContext context = new Models.OrderDBContext())
        {
            ViewData["Items"] = context.Items.OrderBy(i => i.UnitPrice).ToList();
        }
        return View();
    }

    public IActionResult Details(int? id)
    {
        using (Models.OrderDBContext context = new Models.OrderDBContext())
        {
            // ViewData["Item"] = context.Items.Find(id);
            ViewData["Item"] = context.Items.Single(i => i.ItemId == id);
        }
        if (ViewData["Item"] == null)
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
