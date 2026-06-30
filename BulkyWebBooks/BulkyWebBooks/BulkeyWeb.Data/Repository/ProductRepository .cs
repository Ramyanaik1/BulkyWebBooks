using BulkeyWeb.Data.Data;
using BulkeyWeb.Data.Repository.IRepository;
using BulkyWeb.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkeyWeb.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product obj)
        {
            var productFromDb = _db.Products.FirstOrDefault(p => p.Id == obj.Id);
            if (productFromDb != null)
            {
                productFromDb.Title = obj.Title;
                productFromDb.ISBN = obj.ISBN;
                productFromDb.ListPrice = obj.ListPrice;
                productFromDb.Price = obj.Price;
                productFromDb.Price100 = obj.Price100;
                productFromDb.Price50 = obj.Price50;
                productFromDb.Author = obj.Author;
                productFromDb.Description = obj.Description;
                productFromDb.CategoryId = obj.CategoryId;
                productFromDb.ProductImages = obj.ProductImages;
                _db.Update(productFromDb);
            }
        }
    }
}
