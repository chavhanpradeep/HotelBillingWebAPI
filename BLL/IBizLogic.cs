using BLL.Interfaces;

namespace BLL
{
    public interface IBizLogic
    {
        void SaveChanges();
        IMenuItemCommand MenuItemCommand { get; }
        IMenuItemQuery MenuItemQuery { get; }
        IDiscountCommand DiscountCommand { get; }
        IDiscountQuery DiscountQuery { get; }
        IBillCommand BillCommand { get; }
        IBillQuery BillQuery { get; }
    }
}