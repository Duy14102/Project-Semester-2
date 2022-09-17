using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FirstAspNetApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;

namespace FirstAspNetApp.Controllers;

public class AdminController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private OrderDBContext _context;

    public AdminController(ILogger<HomeController> logger, OrderDBContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult AdminPanel()
    {
        if (HttpContext.Session.GetInt32("UserId") != null)
        {
            return View();
        }
        else
        {
            return RedirectToAction("Login");
        }
    }


    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string username, string password)
    {
        if (ModelState.IsValid)
        {
            var session = HttpContext.Session;
            var data = _context.Users.Where(s => s.UserName.Equals(username) && s.Password.Equals(password)).ToList();
            if (data.Count() > 0)
            {
                //add session
                session.SetString("Fullname", data.First().Fullname);
                session.SetString("Email", data.First().Email);
                session.SetInt32("UserId", data.First().UserId);
                return RedirectToAction(nameof(AdminPanel));
            }
            else
            {
                ViewBag.error = "Login failed";
                return RedirectToAction("Login");
            }
        }
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(User _user)
    {
        if (ModelState.IsValid)
        {
            var check = _context.Users.FirstOrDefault(s => s.UserName == _user.UserName);
            if (check == null)
            {
                _context.Users.Add(_user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.error = "Email already exists";
                return View();
            }


        }
        return View();


    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();//remove session
        return RedirectToAction("Login");
    }

    public static string GetMD5(string inputNew)
    {
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            // inputNew = "Palnati"
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(inputNew);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
