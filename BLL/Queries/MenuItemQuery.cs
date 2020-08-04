using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DataObjects;
using BLL.Interfaces;
using DAL;
using DAL.Models;

namespace BLL.Queries
{
    public class MenuItemQuery: IMenuItemQuery
    {
        private IUnitOfWork _unitOfWork;
        public MenuItemQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MenuItemDTO> GetItem(int id)
        {
            var item = await _unitOfWork.MenuItems.GetAsync(id);

            if(item == null)
                throw new Exception(
                    string.Format("item with id ${0} does not exist")
                );
            
            return new MenuItemDTO() {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price
            };
        }

        public async Task<List<MenuItemDTO>> GetAll()
        {
            var items = await _unitOfWork.MenuItems.GetAllAsync();

            var menuItemDTOs = new List<MenuItemDTO>();

            foreach (var item in items.ToList())
            {
                var menuItemDTO = new MenuItemDTO() {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price
                };

                menuItemDTOs.Add(menuItemDTO);
            }
            return menuItemDTOs;
        }
    }
}