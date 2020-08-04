using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.DataObjects;

namespace BLL.Interfaces
{
    public interface IDiscountQuery
    {
        Task<DiscountDTO> Get(int id);
        Task<List<DiscountDTO>> GetAll();
    }
}