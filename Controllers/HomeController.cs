using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FirstAspNetApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAspNetApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly OrderDBContext _context;

    public HomeController(ILogger<HomeController> logger, OrderDBContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        ViewData["Items"] = _context.Items.OrderBy(i => i.ItemId).Take(12).ToList();
        ViewData["Feedbacks"] = _context.Feedbacks.OrderBy(a => a.FeedbackId).ToList();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
