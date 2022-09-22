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
    /*   Admin Panel   */
    public async Task<IActionResult> AdminPanel()
    {
        var y = HttpContext.Session.GetInt32("UserId");
        if (y != null)
        {
            ViewData["Users"] = await _context.Users.OrderBy(i => i.UserId).ToListAsync();
            ViewData["Items"] = await _context.Items.OrderBy(u => u.ItemId).ToListAsync();
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
    /*   Edit For User*/
    [HttpGet]
    public IActionResult EditUser(int id)
    {
        var user = _context.Users.Find(id);
        return View("EditUser", user);
    }

    [HttpPost]
    public async Task<IActionResult> EditUser(int id, User user)
    {
        var data = await _context.Users.FirstOrDefaultAsync(i => i.UserId == id);
        if (data != null)
        {
            data.UserName = user.UserName;
            data.Fullname = user.Fullname;
            data.Email = user.Email;
            data.Role = user.Role;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminPanel));
        }
        return View(data);
    }

    [HttpGet]
    public IActionResult EditItem(int id)
    {
        var item = _context.Items.Find(id);
        return View("EditItem", item);
    }

    [HttpPost]
    public async Task<IActionResult> EditItem(int id, Item item)
    {
        var data = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == id);
        if (data != null)
        {
            data.ItemName = item.ItemName;
            data.UnitPrice = item.UnitPrice;
            data.ItemStory = item.ItemStory;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminPanel));
        }
        return View(data);
    }
    /*   Login Function using Session   */
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
                session.SetInt32("Role", data.First().Role);
                if (data.First().Role == 1)
                {
                    return RedirectToAction(nameof(AdminPanel));
                }
                else if (data.First().Role == 2)
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
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

    public IActionResult Author()
    {
        return View();
    }

    public IActionResult DeleteUser(int id)
    {
        var user = _context.Users.Find(id);
        _context.Users.Remove(user);
        _context.SaveChanges();
        return RedirectToAction("AdminPanel");
    }

    public IActionResult DeleteItem(int id)
    {
        var item = _context.Items.Find(id);
        _context.Items.Remove(item);
        _context.SaveChanges();
        return RedirectToAction("AdminPanel");
    }

    /*   Register from Login Form    */
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
                ViewBag.error = "Username already exists";
                return View();
            }


        }
        return View();
    }
    /*   Logout using Session   */
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();//remove session
        return RedirectToAction("Login");
    }
    /*   Create New Item    */
    [HttpPost]
    public async Task<IActionResult> CreateItem([Bind("ItemName,UnitPrice,Category,ItemStory")] Item value)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _context.Items.Add(value);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AdminPanel));
            }
        }
        catch (DbUpdateException /* ex */)
        {
            //Log the error (uncomment ex variable name and write a log.
            ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists " +
                "see your system administrator.");
        }
        return View(value);
    }
    /*   Create New User    */
    public async Task<IActionResult> EditPost([Bind("UserName, Password, Fullname, Email, Role")] User value)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _context.Users.Add(value);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AdminPanel));
            }
        }
        catch (DbUpdateException /* ex */)
        {
            //Log the error (uncomment ex variable name and write a log.
            ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists " +
                "see your system administrator.");
        }
        return View(value);
    }
    /*   MD5 Script for Password   */
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
    /*   Search Function accross controller for Shared    */
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
