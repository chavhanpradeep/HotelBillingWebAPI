using System;
using System.Linq.Expressions;
using DAL.Models;
namespace DAL.Repositories.Interfaces
{
    public interface IDiscountRepository : IRepository<Discount>
    {
        Discount FindByPredicate(Expression<Func<Discount, bool>> predicate);
    }
}