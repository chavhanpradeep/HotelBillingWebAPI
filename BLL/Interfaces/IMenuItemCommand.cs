using System.Threading.Tasks;
using BLL.DataObjects;

namespace BLL.Interfaces
{
    public interface IMenuItemCommand
    {
        Task AddMenuItem(MenuItemDTO item);
        void Update(MenuItemDTO item);
        Task Delete(int id);
    }
}