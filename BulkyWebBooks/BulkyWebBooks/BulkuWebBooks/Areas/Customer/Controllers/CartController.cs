using BulkeyWeb.Data.Repository;
using BulkeyWeb.Data.Repository.IRepository;
using BulkyWeb.Models.Models;
using BulkyWeb.Models.Models.ViewModels;
using BulkyWeb.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyWebBooks.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitofWork;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitofWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userIdClaim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userIdClaim == null) return Unauthorized();

            ShoppingCartVM shoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCartList = _unitofWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userIdClaim,
                includeProperties: "Product").ToList().Where(u => u.IsOrderPlaced == false),
                OrderHeader = new()
            };
            foreach (var shoppingCart in shoppingCartVM.ShoppingCartList)
            {
                shoppingCart.price = GetPriceBasedOnQuantity(shoppingCart);
                shoppingCartVM.OrderHeader.OrderTotal += (shoppingCart.price * shoppingCart.Count);
            }

            return View(shoppingCartVM);
        }

        public double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
                return shoppingCart.Product.Price;
            else if (shoppingCart.Count <= 100)
                return shoppingCart.Product.Price50;
            else
                return shoppingCart.Product.Price100;
        }

        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitofWork.ShoppingCart.Get(u => u.ShoppingCartId == cartId);
            cartFromDb.Count += 1;
            _unitofWork.ShoppingCart.Update(cartFromDb);
            _unitofWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitofWork.ShoppingCart.Get(u => u.ShoppingCartId == cartId);
            if (cartFromDb.Count <= 1)
            {
                //remove that from cart

                _unitofWork.ShoppingCart.Delete(cartFromDb);
                HttpContext.Session.SetInt32(SD.SessionCart, _unitofWork.ShoppingCart
                    .GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);
            }
            else
            {
                cartFromDb.Count -= 1;
                _unitofWork.ShoppingCart.Update(cartFromDb);
            }

            _unitofWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitofWork.ShoppingCart.Get(u => u.ShoppingCartId == cartId);

            _unitofWork.ShoppingCart.Delete(cartFromDb);

            //HttpContext.Session.SetInt32(SD.SessionCart, _unitofWork.ShoppingCart
            //  .GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);
            _unitofWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartVM ShoppingCartVM = new ShoppingCartVM();
            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitofWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitofWork.ApplicationUser.Get(u => u.Id == userId);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;



            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.price * cart.Count);
            }
            return View(ShoppingCartVM);


        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            // ✅ Materialize the query immediately to avoid streaming conflicts
            ShoppingCartVM.ShoppingCartList = _unitofWork.ShoppingCart
                .GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product")
                .ToList();
          
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

            // ✅ Materialize single entity query
            var applicationUser = _unitofWork.ApplicationUser.Get(u => u.Id == userId);

            // Calculate totals
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.price * cart.Count);
            }

            // Set order status based on user type
            if (applicationUser.CompanyId == 0)
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            }
            else
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            }

            // Save OrderHeader once
            _unitofWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitofWork.Save();

            // ✅ Batch add OrderDetails, then save once
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.IsOrderPlaced = true; // Mark cart as ordered
                cart.UpdatedDateTime = DateTime.Now; // Update timestamp
                var orderDetail = new OrderDetail
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.OrderHeaderId,
                    Price = cart.price,
                    Count = cart.Count
                };
                _unitofWork.ShoppingCart.Update(cart); // Update cart status
                _unitofWork.OrderDetail.Add(orderDetail);
            }
            _unitofWork.Save();

            // Stripe logic (commented out in your code) would go here

            return RedirectToAction(nameof(OrderConfirmation),
                new { id = ShoppingCartVM.OrderHeader.OrderHeaderId });
        }

        public IActionResult OrderConfirmation(int id)
        {

            OrderHeader orderHeader = _unitofWork.OrderHeader.Get(u => u.OrderHeaderId == id, includeProperties: "ApplicationUser");
            //if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
            //{
            //    //this is an order by customer

            //    var service = new SessionService();
            //    Session session = service.Get(orderHeader.SessionId);

            //    if (session.PaymentStatus.ToLower() == "paid")
            //    {
            //        _unitOfWork.OrderHeader.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);
            //        _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
            //        _unitOfWork.Save();
            //    }
            //    HttpContext.Session.Clear();

            //}

            //_emailSender.SendEmailAsync(orderHeader.ApplicationUser.Email, "New Order - Bulky Book",
            //    $"<p>New Order Created - {orderHeader.Id}</p>");

            //List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart
            //    .GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();

            //_unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            //_unitOfWork.Save();

            return View(id);
        }
    }
}