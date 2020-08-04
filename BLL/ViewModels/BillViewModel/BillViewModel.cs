using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.ViewModels
{
    public class BillViewModel
    {
        public BillViewModel()
        {
            BillDetails = new List<BillDetailViewModel>();
        }
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime BillGeneratedOn { get; set; }
        public double DiscountPercentage { get; set; }
        public double DiscountAmount { get; set; }
        public string PDFPath { get; set; }
        public List<BillDetailViewModel> BillDetails { get; set; }
        public double SubTotal
        {
            get
            {
                return BillDetails.Sum(x => x.Price);
            }
        }
        public double SubTotalWithDiscount { get; set; }
        public double VATAmount { get; set; }
        public double TotalPrice { get; set; }

    }
}