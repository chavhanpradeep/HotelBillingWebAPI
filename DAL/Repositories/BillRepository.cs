using System.Collections.Generic;
using System.Linq;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    internal class BillRepository: Repository<Bill>, IBillRepository
    {
        public BillRepository(DbContext context): base(context)
        {
            
        }

        public Task<List<Bill>> GetAllByInclude()
        {
            return _entities
            .Include(bill => bill.Discount)
            .Include(x => x.BillDetails)
            .ToListAsync();
        }

        public Task<Bill> GetByInclude(int id)
        {
            return _entities
            .Include(bill => bill.Discount)
            .Include(x => x.BillDetails)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        }
    }
}