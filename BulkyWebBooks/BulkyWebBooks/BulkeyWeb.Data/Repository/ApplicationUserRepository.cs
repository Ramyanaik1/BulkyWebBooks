using BulkeyWeb.Data.Data;
using BulkeyWeb.Data.Repository.IRepository;
using BulkyWeb.Models.Models;

namespace BulkeyWeb.Data.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ApplicationUser obj)
        {
            _db.Update(obj);
        }
    }
}
