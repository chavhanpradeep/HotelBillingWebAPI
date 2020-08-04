using System.Threading.Tasks;
using DAL.DTOs;

namespace BLL.Interfaces
{
    public interface IBillCommand
    {
        Task Add(BillDTO billDTO);
        Task<string> GetHTMLString(int billId);
    }
}