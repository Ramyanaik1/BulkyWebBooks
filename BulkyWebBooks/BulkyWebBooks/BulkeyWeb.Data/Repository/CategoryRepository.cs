using BulkeyWeb.Data.Data;
using BulkeyWeb.Data.Repository.IRepository;
using BulkyWeb.Models.Models;

namespace BulkeyWeb.Data.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Category obj)
        {
            _db.Update(obj);
            _db.SaveChanges();
        }
    }
}
