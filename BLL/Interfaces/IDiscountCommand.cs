using System.Threading.Tasks;
using BLL.DataObjects;

namespace BLL.Interfaces
{
    public interface IDiscountCommand
    {
        Task Add(DiscountDTO discount);
        void Update(DiscountDTO discount);
        Task Delete(int id);
    }
}