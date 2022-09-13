using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FirstAspNetApp.Models;

namespace FirstAspNetApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        using (Models.OrderDBContext context = new Models.OrderDBContext())
        {
            ViewData["Items"] = context.Items.OrderBy(i => i.ItemId).Take(12).ToList();
            ViewData["Feedbacks"] = context.Feedbacks.OrderBy(a => a.FeedbackId).ToList();
        }
        return View();
    }

    public IActionResult Author()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
