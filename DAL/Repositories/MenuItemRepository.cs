using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    internal class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
    {
        public MenuItemRepository(DbContext context): base(context)
        {
            
        }
    }
}