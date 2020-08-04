using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using DAL.DTOs;
using DAL.Models;
using DAL;
using BLL.Interfaces;
using Utilities;

namespace BLL.Commands
{
    public class BillCommand: IBillCommand
    {
        private IUnitOfWork _unitOfWork;
        public BillCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(BillDTO billDTO)
        {
            var bill = new Bill()
            {
                CustomerName = billDTO.CustomerName,
                CreatedOn = DateTime.Now
            };
            
            bill.BillDetails = AddBillDetails(billDTO.BillDetails);

            var discount = GetDiscountIfApplicable(bill.SubTotal);
            
            if(discount != null) 
                bill.DiscountPercentage = discount.Percentage;
            
            bill.Discount = discount;
            
            await _unitOfWork.Bills.AddAsync(bill);
        }

        public List<BillDetail> AddBillDetails(List<BillDetailDTO> billDetailsDTO)
        {
            var billDetails = new List<BillDetail>();

            foreach (var billDetailDTO in billDetailsDTO)
            {
                var billDetail = AddBillDetail(billDetailDTO);
                billDetails.Add(billDetail);
            }
            return billDetails;
        }

        public BillDetail AddBillDetail(BillDetailDTO billDetail)
        {
            var menuItem = _unitOfWork.MenuItems.Get(billDetail.MenuItemId);

            if(menuItem == null)
                throw new Exception(
                    string.Format("Could not find menu item with id: ${0}", billDetail.MenuItemId)
                );

            return new BillDetail() 
            {
                Id = billDetail.Id,
                ItemName = menuItem.Name,
                Price = menuItem.Price,
                Quantity = billDetail.Quantity
            };
        }

        public Discount GetDiscountIfApplicable(double SubTotal)
        {
            var dayOfWeek = DateTime.Now.DayOfWeek.ToString();

            var discount = _unitOfWork.Discounts
            .FindByPredicate(x => SubTotal >= x.PriceCriteria &&
            x.DayOfWeek.ToLower() == dayOfWeek.ToLower());

            return discount == null ? null : discount;
        }

        public async Task<string> GetHTMLString(int billId)
        {
            var bill = await _unitOfWork.Bills.GetByInclude(billId);
            if(bill == null)
                throw new Exception(
                    string.Format("Could not find bill with id: {0}", billId));

            var templateGenerator = new TemplateGenerator();
            return templateGenerator.GetHTMLString(bill);
        }
    }
}