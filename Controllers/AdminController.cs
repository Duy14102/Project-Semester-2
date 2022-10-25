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
        var x = HttpContext.Session.GetInt32("Role");
        if (y != null)
        {
            if (x == 1)
            {
                ViewData["Users"] = await _context.Users.OrderBy(i => i.UserId).Where(a => a.UserId != y).ToListAsync();
                ViewData["Items"] = await _context.Items.OrderBy(u => u.ItemId).ToListAsync();
                ViewData["Announs"] = await _context.Announs.OrderBy(a => a.AnnounId).ToListAsync();
                ViewData["OHistory"] = await _context.OrderHistories.OrderBy(a => a.OrderHistoryStatus).ToListAsync();
                return View();
            }
            else
            {
                return NotFound();
            }
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

    [HttpPost]
    public async Task<IActionResult> EditHistory(int? id)
    {
        var viewvers = await _context.Histories.FirstOrDefaultAsync(i => i.HistoryOrderId == id);
        var updatethat = 2;
        if (viewvers != null)
        {
            var curse = await _context.OrderHistories.FindAsync(id);
            if (curse != null)
            {
                curse.OrderHistoryStatus = updatethat;
                await _context.SaveChangesAsync();
                return RedirectToAction("AdminPanel");
            }
        }
        return View(viewvers);
    }

    /*   Create New Item    */
    [HttpPost]
    public async Task<IActionResult> EditUser(int id, User user, IFormFile uploadFile)
    {
        var data = await _context.Users.FirstOrDefaultAsync(i => i.UserId == id);
        var checkq = await _context.Users.FirstOrDefaultAsync(a => a.UserName == user.UserName);
        var checkw = await _context.Users.FirstOrDefaultAsync(b => b.Email == user.Email);
        var finduser = _context.Users.Find(id);
        if (data != null)
        {
            if (checkq == null)
            {
                if (checkw == null)
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
                        await _context.SaveChangesAsync();
                        using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                        {
                            await uploadFile.CopyToAsync(fileSrteam);
                        }
                        TempData["EditSuccess"] = "Cập nhật thành công";
                        return View("EditUser", finduser);
                    }
                }
                else
                {
                    TempData["InEmail"] = "Email đã tồn tại";
                    return View("EditUser", finduser);
                }
            }
            else
            {
                TempData["InUsername"] = "Tài khoản đã tồn tại";
                return View("EditUser", finduser);
            }
        }
        return View(data);
    }
    [HttpPost]
    public async Task<IActionResult> EditItem(int id, Item item, IFormFile uploadFile)
    {
        var data = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == id);
        var checkuy = await _context.Items.FirstOrDefaultAsync(a => a.ItemName == item.ItemName);
        var ofwhat = _context.Items.Find(id);
        if (data != null)
        {
            if (checkuy == null)
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
                    TempData["SuccessThisItemname"] = "Cập nhật mặt hàng thành công";
                    return View("EditItem", ofwhat);
                }
            }
            else
            {
                TempData["ErrorThisItemname"] = "Tên mặt hàng đã tồn tại";
                return View("EditItem", ofwhat);
            }
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
        var checkons = HttpContext.Session.GetString("Fullname");
        var onthis = _context.Users.Find(id);
        if (data != null)
        {
            if (checkons == null)
            {
                _context.Users.Remove(data);
                await _context.SaveChangesAsync();
                TempData["DeleteUserSuccess"] = "Xóa người dùng thành công";
                return RedirectToAction(nameof(AdminPanel));
            }
            else
            {
                TempData["WrongDelete"] = "Người dùng đang hoạt động";
                return View("DeleteUser", id);
            }
        }
        return View(data);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteItem(int id, Item item)
    {
        var data = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == id);
        var kuky = _context.Items.Find(id);
        var cart = GetCartItems();
        var cartitem = cart.Find(p => p.item.ItemId == id);
        if (data != null)
        {
            if (cartitem == null)
            {
                _context.Items.Remove(data);
                await _context.SaveChangesAsync();
                TempData["DeleteItemSuccess"] = "Xóa mặt hàng thành công";
                return RedirectToAction(nameof(AdminPanel));
            }
            else
            {
                TempData["Existitem"] = "Không thể xóa mặt hàng vì đang tồn tại trong giỏ hàng";
                return View("DeleteItem", kuky);
            }
        }
        return View(data);
    }
    [HttpPost]
    public async Task<IActionResult> CreateItem([Bind("ItemName,UnitPrice,Category,ItemStory,ItemDescription")] Item value, IFormFile uploadFile)
    {
        try
        {
            var checkfirst = _context.Items.FirstOrDefault(a => a.ItemName == value.ItemName);
            if (checkfirst == null)
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
                    return RedirectToAction(nameof(AdminPanel));
                }
            }
            else
            {
                TempData["DuplicateName"] = "Tên sản phẩm đã tồn tại";
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
    /*   Create New User    */
    public async Task<IActionResult> EditPost([Bind("UserName, Password, Fullname, Email")] User value, IFormFile uploadFile)
    {
        try
        {
            var dood = _context.Users.FirstOrDefault(a => a.Email == value.Email);
            var checkon = _context.Users.FirstOrDefault(a => a.UserName == value.UserName);
            if (checkon == null)
            {
                if (dood == null)
                {
                    if (uploadFile != null && uploadFile.Length > 0)
                    {
                        var fileName = Path.GetFileName(uploadFile.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", fileName);
                        var thisFile = "~/image/" + fileName;
                        value.Image = thisFile;
                        value.Role = 1;
                        _context.Users.Add(value);
                        await _context.SaveChangesAsync();
                        using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                        {
                            await uploadFile.CopyToAsync(fileSrteam);
                        }
                        TempData["errorchecksuccess"] = "Tạo tài khoản thành công";
                        return RedirectToAction(nameof(AdminPanel));
                    }
                }
                else
                {
                    TempData["erroremailcheck"] = "Email đã tồn tại";
                }
            }
            else
            {
                TempData["errorcheckon"] = "Tài khoản đã tồn tại";
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
    public async Task<IActionResult> CreateAnnoun([Bind("AnnounName, AnnounStory")] Announ value)
    {
        try
        {
            var checku = _context.Announs.FirstOrDefault(a => a.AnnounName == value.AnnounName);
            if (checku == null)
            {
                if (ModelState.IsValid)
                {
                    _context.Announs.Add(value);
                    await _context.SaveChangesAsync();
                    TempData["SuccessAnnoun"] = "Tạo thông báo thành công";
                    return RedirectToAction(nameof(AdminPanel));
                }
            }
            else
            {
                TempData["DuplicateAnnoun"] = "Tên thông báo đã tồn tại";
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
        var dood = _context.Users.FirstOrDefault(a => a.Email == _user.Email);
        var check = _context.Users.FirstOrDefault(s => s.UserName == _user.UserName);
        if (check == null)
        {
            if (dood == null)
            {
                if (_user.Password == _user.ConfirmPassword)
                {
                    _context.Users.Add(_user);
                    _context.SaveChanges();
                    TempData["registersuccess"] = "Tạo tài khoản thành công";
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.errorpassword = "Mật khẩu không trùng khớp";
                }
            }
            else
            {
                ViewBag.erroremail = "Email đã tồn tại";
            }
        }
        else
        {
            ViewBag.error = "Tài khoản đã tồn tại";
            return View();
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
    public IActionResult EditHistory(int id)
    {
        var history = _context.Histories.FirstOrDefault(a => a.HistoryOrderId == id);
        ViewData["offerpro"] = _context.OrderHistories.ToList();
        ViewData["Prosbro"] = _context.Histories.ToList();
        return View("EditHistory", history);
    }

    public IActionResult UserEditCartPanel(int id)
    {
        var history = _context.Histories.FirstOrDefault(a => a.HistoryOrderId == id);
        ViewData["offerpro"] = _context.OrderHistories.ToList();
        ViewData["Prosbro"] = _context.Histories.ToList();
        return View("UserEditCartPanel", history);
    }

    public IActionResult UserPanelCart(string id)
    {
        var Orderse = _context.OrderHistories.FirstOrDefault(b => b.OrderHistoryUser == id);
        ViewData["openitrightnow"] = _context.OrderHistories.ToList();
        return View("UserPanelCart", Orderse);
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
