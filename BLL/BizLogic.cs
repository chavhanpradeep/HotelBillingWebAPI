using System;
using BLL.Commands;
using BLL.Queries;
using DAL;
using DAL.Models;
using DAL.Queries;
using BLL.Interfaces;
using DAL.Repositories.Interfaces;

namespace BLL
{
    public class BizLogic: IBizLogic
    {
        private IConfig _config;
        private IUnitOfWork _unitOfWork;
        public BizLogic(IUnitOfWork unitOfWork, IConfig config)
        {
            _config = config;
            _unitOfWork = unitOfWork;
        }

        private IMenuItemCommand _menuItemCommand;
        public IMenuItemCommand MenuItemCommand
        {
            get
            {
                if(_menuItemCommand == null)
                    _menuItemCommand = new MenuItemCommand(_unitOfWork);
                return _menuItemCommand;
            }
        }

        private IMenuItemQuery _menuItemQuery;
        public IMenuItemQuery MenuItemQuery
        {
            get
            {
                if(_menuItemQuery == null)
                    _menuItemQuery = new MenuItemQuery(_unitOfWork);
                return _menuItemQuery;
            }
        }

        private IDiscountCommand _discountCommand;
        public IDiscountCommand DiscountCommand
        {
            get
            {
                if(_discountCommand == null)
                    _discountCommand = new DiscountCommand(_unitOfWork);
                return _discountCommand;
            }
        }

        private IDiscountQuery _discountQuery;
        public IDiscountQuery DiscountQuery
        {
            get
            {
                if(_discountQuery == null)
                    _discountQuery = new DiscountQuery(_unitOfWork);
                return _discountQuery;
            }
        }

        private IBillCommand _billCommand;
        public IBillCommand BillCommand
        {
            get
            {
                if(_billCommand == null)
                    _billCommand = new BillCommand(_unitOfWork);
                return _billCommand;
            }
        }

        private IBillQuery _billQuery;
        public IBillQuery BillQuery
        {
            get
            {
                if(_billQuery == null)
                    _billQuery = new BillQuery(_unitOfWork, _config);
                return _billQuery;
            }
        }

        public void SaveChanges()
        {
            _unitOfWork.SaveChanges();
        }
    }
}
