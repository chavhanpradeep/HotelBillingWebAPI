using DAL.Repositories.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace DAL.Repositories
{
    internal class DiscountRepository : Repository<Discount>, IDiscountRepository
    {

        public DiscountRepository(DbContext context): base(context)
        {
            
        }

        public Discount FindByPredicate(Expression<Func<Discount, bool>> predicate)
        {
            return _entities
            .Where(predicate)
            .FirstOrDefault();
        }
    }
}