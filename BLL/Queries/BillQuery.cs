using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DAL.Models;
using DAL.DTOs;
using BLL.ViewModels;
using BLL.Interfaces;
using DAL.Repositories.Interfaces;
using System.Threading.Tasks;

namespace DAL.Queries
{
    public class BillQuery: IBillQuery
    {
        private IConfig _config;
        private IUnitOfWork _unitOfWork;

        public BillQuery(IUnitOfWork unitOfWork, IConfig config)
        {
            _config = config;
            _unitOfWork = unitOfWork;
        }

        public async Task<BillViewModel> Get(int id)
        {
            var bill = await _unitOfWork.Bills.GetByInclude(id);

            if(bill == null)
                throw new Exception(
                    string.Format("Could not find bill with Id ${0}", id)
                );

            var billViewModel = new BillViewModel() {
                Id = bill.Id,
                CustomerName = bill.CustomerName,
                DiscountPercentage = bill.DiscountPercentage,
                DiscountAmount = bill.DiscountAmount,
                VATAmount = bill.VATAmount,
                TotalPrice = bill.TotalPrice
            };

            return billViewModel;
        }

        public async Task<List<BillViewModel>> GetAll()
        {
            var bills = await _unitOfWork.Bills.GetAllByInclude();
            
            var billsViewModel = new List<BillViewModel>();
            foreach (var bill in bills)
            {
                var billViewModel = new BillViewModel() {
                    Id = bill.Id,
                    CustomerName = bill.CustomerName,
                    DiscountPercentage = bill.DiscountPercentage,
                    DiscountAmount = bill.DiscountAmount,
                    VATAmount = bill.VATAmount,
                    BillGeneratedOn = bill.CreatedOn,
                    SubTotalWithDiscount = bill.SubTotalWithDiscount,
                    TotalPrice = bill.TotalPrice,
                    PDFPath = _config.SavedPDFPath + "\\bill-" + bill.Id + ".pdf"
                };
                billsViewModel.Add(billViewModel);
            }
            return billsViewModel;
        }

        public List<BillDetailViewModel> GetBillDetails(List<BillDetail> billDetails)
        {
            var billDetailsViewModel = new List<BillDetailViewModel>();
            foreach (var billDetail in billDetails)
            {
                var billDetailViewModel = GetBillDetail(billDetail);
                billDetailsViewModel.Add(billDetailViewModel);
            }

            return billDetailsViewModel;
        }

        public BillDetailViewModel GetBillDetail(BillDetail billDetail)
        {
            return new BillDetailViewModel() {
                Id = billDetail.Id,
                ItemName = billDetail.ItemName,
                Price = billDetail.Price,
                Quantity = billDetail.Quantity
            };
        }
    }
}