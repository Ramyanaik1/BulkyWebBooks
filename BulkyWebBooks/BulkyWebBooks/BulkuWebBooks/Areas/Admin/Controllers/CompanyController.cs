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
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitofWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitofWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            //List<Category> categories = _dbContext.Categories.ToList();
            List<Company> companies = _unitofWork.Company.GetAll().ToList();

            return View(companies);
        }

       
        [HttpPost]
        public IActionResult Upsert(Company objCompany)
        {
            if (ModelState.IsValid)
            {
                
                if (objCompany.CompanyId == 0)
                {
                    _unitofWork.Company.Add(objCompany);
                    TempData["success"] = "Product Created Successfully";

                }
                else
                {
                    _unitofWork.Company.Update(objCompany);
                    TempData["success"] = "Product Updated Successfully";

                }
                _unitofWork.Save();
                return RedirectToAction("Index");
            }
            
            return View(objCompany);
        }

        /// <summary>
        /// Method used to Create and Update the Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public IActionResult Upsert(int? id)
        {


           
            if (id == null || id == 0)
            {
                Company company = new Company();
                return View(company);
            }
            Company objCompany = _unitofWork.Company.Get(i => i.CompanyId == id);
            return View(objCompany);

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
