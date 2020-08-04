// =============================
// claritytechnologies
// Tallify
// =============================

using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IUnitOfWork
    {
        int SaveChanges();
        IMenuItemRepository MenuItems { get; }
        IDiscountRepository Discounts { get; }
        IBillRepository Bills { get; }
    }
}
