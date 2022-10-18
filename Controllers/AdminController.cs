using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FirstAspNetApp.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using System.Text;
using Newtonsoft.Json;

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
            ViewData["Announs"] = await _context.Announs.OrderBy(a => a.AnnounId).ToListAsync();
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

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult Author()
    {
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
    public async Task<IActionResult> EditUser(int id, User user, IFormFile uploadFile)
    {
        var data = await _context.Users.FirstOrDefaultAsync(i => i.UserId == id);
        if (data != null)
        {
            if (uploadFile != null && uploadFile.Length > 0)
            {
                var fileName = Path.GetFileName(uploadFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", fileName);
                data.Image = "~/image/" + fileName;
                data.UserName = user.UserName;
                data.Password = user.Password;
                data.Fullname = user.Fullname;
                data.Email = user.Email;
                data.Role = user.Role;
                await _context.SaveChangesAsync();
                using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                {
                    await uploadFile.CopyToAsync(fileSrteam);
                }
                return RedirectToAction(nameof(AdminPanel));
            }
        }
        return View(data);
    }
    [HttpPost]
    public async Task<IActionResult> EditItem(int id, Item item, IFormFile uploadFile)
    {
        var data = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == id);
        if (data != null)
        {
            if (uploadFile != null && uploadFile.Length > 0)
            {
                var fileName = Path.GetFileName(uploadFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", fileName);
                data.ItemDescription = "~/image/" + fileName;
                data.Category = item.Category;
                data.ItemName = item.ItemName;
                data.UnitPrice = item.UnitPrice;
                data.ItemStory = item.ItemStory;
                await _context.SaveChangesAsync();
                using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                {
                    await uploadFile.CopyToAsync(fileSrteam);
                }
            }
            return RedirectToAction(nameof(AdminPanel));
        }
        return View(data);
    }
    [HttpPost]
    public async Task<IActionResult> EditUserPanel(int id, User user, IFormFile uploadFile)
    {
        var data = await _context.Users.FirstOrDefaultAsync(i => i.UserId == id);
        if (data != null)
        {
            if (uploadFile != null && uploadFile.Length > 0)
            {
                var fileName = Path.GetFileName(uploadFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", fileName);
                data.Image = "~/image/" + fileName;
                data.Password = user.Password;
                data.Fullname = user.Fullname;
                data.Email = user.Email;
                await _context.SaveChangesAsync();
                using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                {
                    await uploadFile.CopyToAsync(fileSrteam);
                }
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        return View(data);
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
                TempData["error"] = "Sai tên tài khoản hoặc mật khẩu. Vui lòng thử lại";
                return RedirectToAction("Login", "Admin");
            }
        }
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> DeleteUser(int id, User user)
    {
        var data = await _context.Users.FirstOrDefaultAsync(i => i.UserId == id);
        if (data != null)
        {
            _context.Users.Remove(data);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminPanel));
        }
        return View(data);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteItem(int id, Item item)
    {
        var data = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == id);
        if (data != null)
        {
            _context.Items.Remove(data);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminPanel));
        }
        return View(data);
    }
    [HttpPost]
    public async Task<IActionResult> CreateItem([Bind("ItemName,UnitPrice,Category,ItemStory,ItemDescription")] Item value, IFormFile uploadFile)
    {
        try
        {
            if (uploadFile != null && uploadFile.Length > 0)
            {
                var fileName = Path.GetFileName(uploadFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", fileName);
                var thisFile = "~/image/" + fileName;
                value.ItemDescription = thisFile;
                _context.Items.Add(value);
                await _context.SaveChangesAsync();
                using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                {
                    await uploadFile.CopyToAsync(fileSrteam);
                }
            }
            return RedirectToAction(nameof(AdminPanel));
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
    [HttpPost]
    /*   Create New User    */
    public async Task<IActionResult> EditPost([Bind("UserName, Password, Fullname, Email, Role")] User value, IFormFile uploadFile)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                if (uploadFile != null && uploadFile.Length > 0)
                {
                    var fileName = Path.GetFileName(uploadFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", fileName);
                    var thisFile = "~/image/" + fileName;
                    value.Image = thisFile;
                    _context.Users.Add(value);
                    await _context.SaveChangesAsync();
                    using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                    {
                        await uploadFile.CopyToAsync(fileSrteam);
                    }
                }
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
    [HttpPost]
    public async Task<IActionResult> CreateAnnoun([Bind("AnnounName, AnnounStory")] Announ value)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _context.Announs.Add(value);
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
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(User _user)
    {
        if (!ModelState.IsValid)
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
    /*   Search Function accross controller for Shared    */
    [HttpGet]
    public IActionResult Search(string searchString, string sortOrder, int? page = 1)
    {
        if (page != null && page < 1)
        {
            page = 1;
        }
        var pageSize = 12;
        var movies = _context.Items.ToPagedList();
        if (!String.IsNullOrEmpty(searchString))
        {
            movies = _context.Items.Where(s => s.ItemName.Contains(searchString)).OrderBy(i => i.ItemId).ToPagedList(page ?? 1, pageSize);
        }
        else if (searchString == null)
        {
            return RedirectToAction("TatCa");
        }
        switch (sortOrder)
        {
            // 3.2 Mặc định thì sẽ sắp tăng
            default:
                movies = _context.Items.Where(s => s.ItemName.Contains(searchString)).OrderBy(b => b.ItemId).ToPagedList(page ?? 1, pageSize);
                break;
            case "idnewsearch":
                movies = _context.Items.Where(s => s.ItemName.Contains(searchString)).OrderByDescending(b => b.ItemId).ToPagedList(page ?? 1, pageSize);
                break;
            case "idoldsearch":
                movies = _context.Items.Where(s => s.ItemName.Contains(searchString)).OrderBy(b => b.ItemId).ToPagedList(page ?? 1, pageSize);
                break;
            // 3.2 Sắp tăng dần theo price
            case "priceupsearch":
                movies = _context.Items.Where(s => s.ItemName.Contains(searchString)).OrderBy(b => b.UnitPrice).ToPagedList(page ?? 1, pageSize);
                break;
            // 3.4 Sắp giảm theo price
            case "pricedownsearch":
                movies = _context.Items.Where(s => s.ItemName.Contains(searchString)).OrderByDescending(b => b.UnitPrice).ToPagedList(page ?? 1, pageSize);
                break;
            case "categorysearch":
                movies = _context.Items.Where(s => s.ItemName.Contains(searchString)).OrderBy(b => b.Category).ToPagedList(page ?? 1, pageSize);
                break;
        }
        ViewBag.OrderTimkiem = sortOrder;
        ViewBag.Message = searchString;
        return View(movies);
    }
    public IActionResult DeleteUser(int id)
    {
        var user = _context.Users.Find(id);
        return View("DeleteUser", user);
    }
    public IActionResult DeleteItem(int id)
    {
        var item = _context.Items.Find(id);
        return View("DeleteItem", item);
    }
    public IActionResult EditItem(int id)
    {
        var item = _context.Items.Find(id);
        return View("EditItem", item);
    }
    public IActionResult EditUser(int id)
    {
        var user = _context.Users.Find(id);
        return View("EditUser", user);
    }
    public IActionResult UserPanel(int id)
    {
        var user = _context.Users.Find(id);
        return View("UserPanel", user);
    }
    public IActionResult EditUserPanel(int id)
    {
        var user = _context.Users.Find(id);
        return View("EditUserPanel", user);
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
