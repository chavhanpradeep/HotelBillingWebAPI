using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.DataObjects;

namespace BLL.Interfaces
{
    public interface IMenuItemQuery
    {
        Task<MenuItemDTO> GetItem(int id);
        Task<List<MenuItemDTO>> GetAll();
    }
}