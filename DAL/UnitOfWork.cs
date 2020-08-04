// =============================
// claritytechnologies
// Tallify
// =============================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories;
using DAL.Repositories.Interfaces;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly ApplicationDbContext _context;

    #region Base
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    #endregion

        private IMenuItemRepository _menuItems;
        public IMenuItemRepository MenuItems 
        { 
            get
            {
                if(_menuItems == null)
                    _menuItems = new MenuItemRepository(_context);
                return _menuItems;
            }
        }

        private IDiscountRepository _discounts;
        public IDiscountRepository Discounts
        {
            get
            {
                if(_discounts == null)
                    _discounts = new DiscountRepository(_context);
                return _discounts;
            }
        }

        private IBillRepository _bills;
        public IBillRepository Bills
        {
            get
            {
                if(_bills == null)
                    _bills = new BillRepository(_context);
                return _bills;
            }
        }
    }
}
