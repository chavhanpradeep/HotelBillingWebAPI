using System.Collections.Generic;
using DAL.Models;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IBillRepository: IRepository<Bill>
    {
        Task<Bill> GetByInclude(int id);
        Task<List<Bill>> GetAllByInclude();
    }
}