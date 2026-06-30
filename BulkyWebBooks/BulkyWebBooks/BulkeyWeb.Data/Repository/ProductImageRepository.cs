using BulkeyWeb.Data.Data;
using BulkeyWeb.Data.Repository.IRepository;
using BulkyWeb.Models.Models;

namespace BulkeyWeb.Data.Repository
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        private ApplicationDbContext _db;
        public ProductImageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ProductImage obj)
        {
            _db.Update(obj);
            _db.SaveChanges();
        }
    }
}
