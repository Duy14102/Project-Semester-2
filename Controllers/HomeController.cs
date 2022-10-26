using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FirstAspNetApp.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using Newtonsoft.Json;

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
        var data = ViewData["Items"];
        ViewData["Feedbacks"] = _context.Feedbacks.OrderBy(a => a.FeedbackId).ToList();
        return View(data);
    }

    /*     Shopping Cart     */
    //Add Cart
    [Route("addcart/{ItemId:int}", Name = "addcart")]
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
        TempData["CartPopup2"] = "Thêm giỏ hàng thành công";
        // Lưu cart vào Session
        SaveCartSession(cart);
        // Chuyển đến trang hiện thị Cart
        return RedirectToAction(nameof(Index));
    }


    // Hiện thị giỏ hàng
    public IActionResult Cart()
    {
        return View(GetCartItems());
    }

    /// Update Cart
    [Route("/updatecart", Name = "updatecart")]
    [HttpPost]
    public IActionResult UpdateCart([FromForm] int ItemId, [FromForm] int Quantity)
    {
        // Cập nhật Cart thay đổi số lượng quantity ...
        var cart = GetCartItems();
        var cartitem = cart.Find(p => p.item.ItemId == ItemId);
        if (cartitem != null)
        {
            // Đã tồn tại, tăng thêm 1
            cartitem.Quantity = Quantity;
        }
        TempData["updatesuccess"] = "Cập nhật giỏ hàng thành công";
        SaveCartSession(cart);
        // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
        return Ok();
    }

    /// xóa item trong cart
    [Route("/removecart/{ItemId:int}", Name = "removecart")]
    public IActionResult RemoveCart([FromRoute] int ItemId)
    {
        var cart = GetCartItems();
        var cartitem = cart.Find(p => p.item.ItemId == ItemId);
        if (cartitem != null)
        {
            // Đã tồn tại, tăng thêm 1
            cart.Remove(cartitem);
        }
        TempData["removesuccess"] = "Xóa mặt hàng thành công";
        SaveCartSession(cart);
        return RedirectToAction(nameof(Cart));
    }

    //Checkout
    [HttpPost]
    [Route("/Checkoutmain", Name = "Checkoutmain")]
    public IActionResult Checkout([Bind("HistoryFullname,HistoryEmail,HistoryAddress,HistoryPhone,HistoryNote")] History he)
    {
        try
        {
            var calmdown = HttpContext.Session;
            var getitem = GetCartItems();
            if (getitem == null)
            {
                return View("Index");
            }
            var getuser = HttpContext.Session.GetString("Fullname");
            var forgetit = "(Khách vãng lai)";
            OrderHistory ohi = new OrderHistory();
            History hi;
            if (getuser == null)
            {
                ohi.OrderHistoryUser = he.HistoryFullname;
                _context.OrderHistories.Add(ohi);
                _context.SaveChanges();
                foreach (var value in getitem)
                {
                    hi = new History();
                    hi.HistoryFullname = he.HistoryFullname + forgetit;
                    hi.HistoryEmail = he.HistoryEmail;
                    hi.HistoryAddress = he.HistoryAddress;
                    hi.HistoryPhone = he.HistoryPhone;
                    hi.HistoryQuantity = value.Quantity;
                    hi.HistoryName = value.item.ItemName;
                    hi.HistoryPrice = value.item.UnitPrice;
                    hi.HistoryOrderId = ohi.OrderHistoryID;
                    _context.Histories.Add(hi);
                }
                _context.SaveChanges();
            }
            else
            {
                ohi.OrderHistoryUser = getuser;
                _context.OrderHistories.Add(ohi);
                _context.SaveChanges();
                foreach (var value in getitem)
                {
                    hi = new History();
                    hi.HistoryFullname = calmdown.GetString("Fullname");
                    hi.HistoryEmail = calmdown.GetString("Email");
                    hi.HistoryAddress = calmdown.GetString("Address");
                    hi.HistoryPhone = calmdown.GetString("Phone");
                    hi.HistoryQuantity = value.Quantity;
                    hi.HistoryName = value.item.ItemName;
                    hi.HistoryPrice = value.item.UnitPrice;
                    hi.HistoryOrderId = ohi.OrderHistoryID;
                    _context.Histories.Add(hi);
                }
                _context.SaveChanges();
            }
            ClearCart();
            return View();
        }
        catch (DbUpdateException /* ex */)
        {
            //Log the error (uncomment ex variable name and write a log.
            ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists " +
                "see your system administrator.");
        }
        return View("Index");
    }

    [Route("/CheckoutDetail", Name = "CheckoutDetail")]
    public IActionResult CheckoutDetail()
    {
        ViewData["Gotyoubaby"] = _context.Histories.ToList();
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

    // Xóa cart khỏi session
    void ClearCart()
    {
        var session = HttpContext.Session;
        session.Remove(CARTKEY);
    }

    // Lưu Cart (Danh sách CartItem) vào session
    void SaveCartSession(List<CartItem> ls)
    {
        var session = HttpContext.Session;
        string jsoncart = JsonConvert.SerializeObject(ls);
        session.SetString(CARTKEY, jsoncart);
    }

    public IActionResult Privacy()
    {
        ViewData["Announs"] = _context.Announs.OrderBy(i => i.AnnounId).ToList();
        return View();
    }

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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
