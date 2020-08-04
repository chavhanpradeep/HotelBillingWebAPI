using System.Collections.Generic;
using BLL.ViewModels;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IBillQuery
    {
        Task<BillViewModel> Get(int id);
        Task<List<BillViewModel>> GetAll();
    }
}