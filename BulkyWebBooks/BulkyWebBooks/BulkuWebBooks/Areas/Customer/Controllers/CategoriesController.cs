
using BulkeyWeb.Data.Data;
using BulkeyWeb.Data.Repository;
using BulkeyWeb.Data.Repository.IRepository;
using BulkyWeb.Models.Models;
using BulkyWeb.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWebBooks.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unitofWork;
        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitofWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //List<Category> categories = _dbContext.Categories.ToList();
            List<Category> categories = _unitofWork.Category.GetAll().ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitofWork.Category.Add(category);
                _unitofWork.Save();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _unitofWork.Category.Get(i => i.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        public IActionResult Update(Category objCategory)
        {
            if (objCategory != null && objCategory.CategoryId > 0)
            {
                _unitofWork.Category.Update(objCategory);
            }
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _unitofWork.Category.Get(i => i.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _unitofWork.Category.Get(u => u.CategoryId == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitofWork.Category.Delete(obj);
            _unitofWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }

    }
}
