using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FirstAspNetApp.Models;

namespace FirstAspNetApp.Controllers;

public class AdminController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public AdminController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Signup()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
