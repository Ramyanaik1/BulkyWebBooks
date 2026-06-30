using BulkeyWeb.Data.Data;
using BulkeyWeb.Data.Repository;
using BulkeyWeb.Data.Repository.IRepository;
using BulkyWeb.Models.Models;
using BulkyWeb.Models.ViewModels;
using BulkyWeb.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Language;
using System.Collections.Generic;

namespace BulkyWebBooks.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitofWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitofWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            //List<Category> categories = _dbContext.Categories.ToList();
            List<Product> products = _unitofWork.Product.GetAll(includeProperties:"Category").ToList();

            return View(products);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> selectListItems = _unitofWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.CategoryName,
                Value = u.CategoryId.ToString()
            });
            ProductVM productVM = new()
            {

                CategoryList = selectListItems.ToList(),

                Product = new Product()
            };
            return View(productVM);
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM objProduct, IEnumerable<IFormFile?> files)
        {
            if (ModelState.IsValid)
            {
                if (objProduct.Product.Id == 0)
                {
                    _unitofWork.Product.Add(objProduct.Product);
                }
                else
                {
                    _unitofWork.Product.Update(objProduct.Product);
                }

                _unitofWork.Save();


                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (files != null)
                {

                    foreach (IFormFile file in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = @"images\products\product-" + objProduct.Product.Id;
                        string finalPath = Path.Combine(wwwRootPath, productPath);

                        if (!Directory.Exists(finalPath))
                            Directory.CreateDirectory(finalPath);

                        using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        ProductImage productImage = new()
                        {
                            ImageUrl = @"\" + productPath + @"\" + fileName,
                            ProductId = objProduct.Product.Id,
                        };

                        if (objProduct.Product.ProductImages == null)
                            objProduct.Product.ProductImages = new List<ProductImage>();

                        objProduct.Product.ProductImages.Add(productImage);

                    }

                    _unitofWork.Product.Update(objProduct.Product);
                    _unitofWork.Save();




                }


                TempData["success"] = "Product created/updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                objProduct.CategoryList = _unitofWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.CategoryName,
                    Value = u.CategoryId.ToString()
                }).ToList();
                return View(objProduct);
            }
        }

        /// <summary>
        /// Method used to Create and Update the Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitofWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.CategoryName,
                    Value = u.CategoryId.ToString()
                }).ToList(),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitofWork.Product.Get(u => u.Id == id, includeProperties: "ProductImages");
                return View(productVM);
            }

        }

        public IActionResult Update(Product objProduct)
        {
            if (objProduct != null && objProduct.Id > 0)
            {
                _unitofWork.Product.Update(objProduct);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var product = _unitofWork.Product.Get(i => i.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitofWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitofWork.Product.Delete(obj);
            _unitofWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }

        public IActionResult DeleteImage(int imageId)
        {
            var imageToBeDeleted = _unitofWork.ProductImage.Get(u => u.Id == imageId);
            int productId = imageToBeDeleted.ProductId;
            if (imageToBeDeleted != null)
            {
                if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
                {
                    var oldImagePath =
                                   Path.Combine(_webHostEnvironment.WebRootPath,
                                   imageToBeDeleted.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitofWork.ProductImage.Delete(imageToBeDeleted);
                _unitofWork.Save();

                TempData["success"] = "Deleted successfully";
            }

            return RedirectToAction(nameof(Upsert), new { id = productId });
        }


        #region for API CALLS 
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = _unitofWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new {data =  products});
        }
        #endregion


    }
    }
