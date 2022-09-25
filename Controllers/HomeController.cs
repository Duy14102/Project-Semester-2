using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FirstAspNetApp.Models;
using Microsoft.EntityFrameworkCore;
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

        // Lưu cart vào Session
        SaveCartSession(cart);
        // Chuyển đến trang hiện thị Cart
        return RedirectToAction(nameof(Cart));
    }

    // Hiện thị giỏ hàng
    [Route("/cart", Name = "cart")]
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

        SaveCartSession(cart);
        return RedirectToAction(nameof(Cart));
    }

    //Checkout
    [Route("/checkout")]
    public IActionResult CheckOut()
    {
        ClearCart();
        return View();
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
