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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Company obj)
        {
            _db.Update(obj);
            _db.SaveChanges();
        }
    }
}
