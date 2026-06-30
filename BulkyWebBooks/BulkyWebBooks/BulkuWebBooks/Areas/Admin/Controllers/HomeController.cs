using BulkeyWeb.Data.Repository.IRepository;
using BulkuWebBooks.Models;
using BulkyWeb.Models.Models;
using BulkyWeb.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyWebBooks.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitofWork;
        public HomeController(IUnitOfWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public IActionResult Books()
        {
            List<Product> products = _unitofWork.Product.GetAll(includeProperties: "Category,ProductImages").ToList();

            return View(products);
        }

        public IActionResult Details(int id)
        {
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Product = _unitofWork.Product.Get(u => u.Id == id, includeProperties: "Category,ProductImages"),
                ProductId = id
            };
            if (shoppingCart.IsOrderPlaced == true) //  If order placed already then create new shopping cart object
            {
                shoppingCart = new ShoppingCart();
                shoppingCart.Product = _unitofWork.Product.Get(u => u.Id == id, includeProperties: "Category");
                shoppingCart.ProductId = id;
            }


            return View(shoppingCart);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Details(ShoppingCart objShoppingCart)
        {

            objShoppingCart.Product = _unitofWork.Product.Get(
                u => u.Id == objShoppingCart.ProductId, includeProperties: "Category");
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userIdClaim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            ShoppingCart shoppingCartFrmDb = new ShoppingCart();
            if (objShoppingCart != null && objShoppingCart.ShoppingCartId != 0)
            {
                shoppingCartFrmDb = _unitofWork.ShoppingCart.Get(
                u => u.ApplicationUserId == userIdClaim.Value && u.ProductId == objShoppingCart.ProductId && u.ShoppingCartId == objShoppingCart.ShoppingCartId 
                && objShoppingCart.IsOrderPlaced == false);
            }

            if (shoppingCartFrmDb == null || shoppingCartFrmDb.ShoppingCartId == 0)
            {
                objShoppingCart.ApplicationUserId = userIdClaim.Value;
                _unitofWork.ShoppingCart.Add(objShoppingCart);
                _unitofWork.Save();

                HttpContext.Session.SetInt32(SD.SessionCart, _unitofWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userIdClaim.Value).ToList().Count);
            }
            else
            {
                shoppingCartFrmDb.Count += objShoppingCart.Count;
                _unitofWork.ShoppingCart.Update(shoppingCartFrmDb);
                _unitofWork.Save();
            }
            return RedirectToAction("Books");

        }
    }
}

