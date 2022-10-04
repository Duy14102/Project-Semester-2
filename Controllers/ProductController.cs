using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FirstAspNetApp.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using Newtonsoft.Json;

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

    public IActionResult TatCa(string sortOrder, int? page = 1)
    {
        if (page != null && page < 1)
        {
            page = 1;
        }
        var pageSize = 12;
        var item = _context.Items.ToPagedList();
        // ViewData["Items"] = context.Items.OrderBy(i => i.ItemId).ToList();
        switch (sortOrder)
        {
            // 3.2 Mặc định thì sẽ sắp tăng
            default:
                item = _context.Items.OrderBy(b => b.ItemId).ToPagedList(page ?? 1, pageSize);
                break;
            case "idnew":
                item = _context.Items.OrderByDescending(b => b.ItemId).ToPagedList(page ?? 1, pageSize);
                break;
            case "idold":
                item = _context.Items.OrderBy(b => b.ItemId).ToPagedList(page ?? 1, pageSize);
                break;
            // 3.2 Sắp tăng dần theo price
            case "priceup":
                item = _context.Items.OrderBy(b => b.UnitPrice).ToPagedList(page ?? 1, pageSize);
                break;
            // 3.4 Sắp giảm theo price
            case "pricedown":
                item = _context.Items.OrderByDescending(b => b.UnitPrice).ToPagedList(page ?? 1, pageSize);
                break;
            case "category":
                item = _context.Items.OrderBy(b => b.Category).ToPagedList(page ?? 1, pageSize);
                break;
        }
        return View(item);
    }

    public IActionResult DoAnKem(string sortOrder, int? page = 1)
    {
        string water = "Đồ Ăn Kèm";
        if (page != null && page < 1)
        {
            page = 1;
        }
        var pageSize = 12;
        var item = _context.Items.OrderBy(i => i.ItemId).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
        switch (sortOrder)
        {
            // 3.2 Mặc định thì sẽ sắp tăng
            default:
                item = _context.Items.OrderBy(b => b.ItemId).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            case "idnewdak":
                item = _context.Items.OrderByDescending(b => b.ItemId).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            case "idolddak":
                item = _context.Items.OrderBy(b => b.ItemId).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            // 3.2 Sắp tăng dần theo price
            case "priceupdak":
                item = _context.Items.OrderBy(b => b.UnitPrice).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            // 3.4 Sắp giảm theo price
            case "pricedowndak":
                item = _context.Items.OrderByDescending(b => b.UnitPrice).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            case "categorydak":
                item = _context.Items.OrderBy(b => b.Category).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
        }
        return View(item);
    }

    public IActionResult Thit(string sortOrder, int? page = 1)
    {
        string water = "Thịt";
        if (page != null && page < 1)
        {
            page = 1;
        }
        var pageSize = 12;
        var item = _context.Items.OrderBy(i => i.ItemId).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
        switch (sortOrder)
        {
            // 3.2 Mặc định thì sẽ sắp tăng
            default:
                item = _context.Items.OrderBy(b => b.ItemId).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            case "idnewt":
                item = _context.Items.OrderByDescending(b => b.ItemId).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            case "idoldt":
                item = _context.Items.OrderBy(b => b.ItemId).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            // 3.2 Sắp tăng dần theo price
            case "priceupt":
                item = _context.Items.OrderBy(b => b.UnitPrice).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            // 3.4 Sắp giảm theo price
            case "pricedownt":
                item = _context.Items.OrderByDescending(b => b.UnitPrice).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            case "categoryt":
                item = _context.Items.OrderBy(b => b.Category).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
        }
        return View(item);
    }

    public IActionResult Rau(string sortOrder, int? page = 1)
    {
        string water = "Rau";
        if (page != null && page < 1)
        {
            page = 1;
        }
        var pageSize = 12;
        var item = _context.Items.OrderBy(i => i.ItemId).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
        switch (sortOrder)
        {
            // 3.2 Mặc định thì sẽ sắp tăng
            default:
                item = _context.Items.OrderBy(b => b.ItemId).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            case "idnewr":
                item = _context.Items.OrderByDescending(b => b.ItemId).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            case "idoldr":
                item = _context.Items.OrderBy(b => b.ItemId).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            // 3.2 Sắp tăng dần theo price
            case "priceupr":
                item = _context.Items.OrderBy(b => b.UnitPrice).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            // 3.4 Sắp giảm theo price
            case "pricedownr":
                item = _context.Items.OrderByDescending(b => b.UnitPrice).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            case "categoryr":
                item = _context.Items.OrderBy(b => b.Category).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
        }
        return View(item);
    }

    public IActionResult Canh(string sortOrder, int? page = 1)
    {
        string water = "Canh";
        if (page != null && page < 1)
        {
            page = 1;
        }
        var pageSize = 12;
        var item = _context.Items.OrderBy(i => i.ItemId).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
        switch (sortOrder)
        {
            // 3.2 Mặc định thì sẽ sắp tăng
            default:
                item = _context.Items.OrderBy(b => b.ItemId).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            case "idnewc":
                item = _context.Items.OrderByDescending(b => b.ItemId).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            case "idoldc":
                item = _context.Items.OrderBy(b => b.ItemId).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            // 3.2 Sắp tăng dần theo price
            case "priceupc":
                item = _context.Items.OrderBy(b => b.UnitPrice).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            // 3.4 Sắp giảm theo price
            case "pricedownc":
                item = _context.Items.OrderByDescending(b => b.UnitPrice).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            case "categoryc":
                item = _context.Items.OrderBy(b => b.Category).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
        }
        return View(item);
    }

    public IActionResult Nuoc(string sortOrder, int? page = 1)
    {
        string water = "Nước";
        if (page != null && page < 1)
        {
            page = 1;
        }
        var pageSize = 12;
        var item = _context.Items.OrderBy(i => i.ItemId).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
        switch (sortOrder)
        {
            // 3.2 Mặc định thì sẽ sắp tăng
            default:
                item = _context.Items.OrderBy(b => b.ItemId).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            case "idnewn":
                item = _context.Items.OrderByDescending(b => b.ItemId).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            case "idoldn":
                item = _context.Items.OrderBy(b => b.ItemId).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            // 3.2 Sắp tăng dần theo price
            case "priceupn":
                item = _context.Items.OrderBy(b => b.UnitPrice).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            // 3.4 Sắp giảm theo price
            case "pricedownn":
                item = _context.Items.OrderByDescending(b => b.UnitPrice).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
            case "categoryn":
                item = _context.Items.OrderBy(b => b.Category).Where(i => i.Category == water).ToPagedList(page ?? 1, pageSize);
                break;
        }
        return View(item);
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

    [HttpPost]
    // [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FeedbackName,FeedbackStory")] Feedback feedback)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TatCa));
            }
        }
        catch (DbUpdateException /* ex */)
        {
            //Log the error (uncomment ex variable name and write a log.
            ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists " +
                "see your system administrator.");
        }
        return View(feedback);
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
        else if (searchString == null)
        {
            return RedirectToAction("TatCa");
        }
        ViewBag.Message = searchString;
        return View(await movies.ToListAsync());
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

    [Route("addcartProduct/{ItemId:int}", Name = "addcartProduct")]
    public IActionResult AddToCart([FromRoute] int ItemId)
    {

        var data = _context.Items
            .Where(p => p.ItemId == ItemId)
            .FirstOrDefault();
        if (data == null)
            return NotFound("Không có sản phẩm");

        // Xử lý đưa vào Cart ...
        var cart = GetCartItems();
        var cartitem = cart.Find(p => p.item.ItemId == ItemId);
        if (cartitem != null)
        {
            // Đã tồn tại, tăng thêm 1
            cartitem.Quantity++;
        }
        else
        {
            //  Thêm mới
            cart.Add(new CartItem() { Quantity = 1, item = data });
        }
        TempData["CartPopup2"] = "Sản phẩm đã được thêm vào giỏ hàng";
        // Lưu cart vào Session
        SaveCartSession(cart);
        // Chuyển đến trang hiện thị Cart
        return RedirectToAction(nameof(TatCa));
    }

    [Route("addcartProductDetail/{ItemId:int}", Name = "addcartProductDetail")]
    public IActionResult AddToCartDetail([FromRoute] int ItemId, int number45)
    {

        var data = _context.Items
            .Where(p => p.ItemId == ItemId)
            .FirstOrDefault();
        if (data == null)
            return NotFound("Không có sản phẩm");

        // Xử lý đưa vào Cart ...
        var cart = GetCartItems();
        var cartitem = cart.Find(p => p.item.ItemId == ItemId);
        if (cartitem != null)
        {
            // Đã tồn tại, tăng thêm 1
            cartitem.Quantity++;
        }
        else
        {
            //  Thêm mới
            cart.Add(new CartItem() { Quantity = number45, item = data });
        }
        // Lưu cart vào Session
        SaveCartSession(cart);
        // Chuyển đến trang hiện thị Cart
        return RedirectToAction(nameof(cart));
    }

    void SaveCartSession(List<CartItem> ls)
    {
        var session = HttpContext.Session;
        string jsoncart = JsonConvert.SerializeObject(ls);
        session.SetString(CARTKEY, jsoncart);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
