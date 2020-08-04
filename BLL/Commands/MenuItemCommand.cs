using System;
using DAL;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using DAL.Models;
using BLL.Interfaces;
using BLL.DataObjects;

namespace BLL.Commands
{
    public class MenuItemCommand: IMenuItemCommand
    {
        private IUnitOfWork _unitOfWork;
        public MenuItemCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddMenuItem(MenuItemDTO item)
        {
            var menuItem = new MenuItem() {
                Name = item.Name,
                Price = item.Price
            };
            _unitOfWork.MenuItems.AddAsync(menuItem);
        }

        public void UpdateMany(List<MenuItem> items)
        {
            foreach (MenuItem item in items)
            {
                var existingItem = _unitOfWork.MenuItems.Get(item.Id);

                if (existingItem == null)
                {
                    _unitOfWork.MenuItems.Add(item);
                    continue;
                }
                else
                {
                    existingItem.Name = item.Name;
                    existingItem.Price = item.Price;
                    _unitOfWork.MenuItems.Update(existingItem);
                }

                var allItems = _unitOfWork.MenuItems.GetAll()
                    .Where(x => x.Id != item.Id)
                    .ToList();

                foreach (var menuItem in allItems)
                {
                    _unitOfWork.MenuItems.Remove(menuItem);
                }
            }
        }

        public void Update(MenuItemDTO item)
        {
            var existingItem = _unitOfWork.MenuItems.Get(item.Id);

            if (existingItem == null)
            {
                throw new Exception(
                    string.Format(
                        "Could not find menu item with id: ${0}", item.Id
                    ));
            }

            existingItem.Name = item.Name;
            existingItem.Price = item.Price;

            _unitOfWork.MenuItems.Update(existingItem);
        }

        public async Task Delete(int id)
        {
            var existingItem = await _unitOfWork.MenuItems.GetAsync(id);

            if(existingItem == null)
                throw new Exception(
                    string.Format("item with id ${0} has already been deleted", id)
                );
            else
                _unitOfWork.MenuItems.Remove(existingItem);
        }
    }
}