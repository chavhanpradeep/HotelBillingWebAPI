using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Models
{
    public class Bill
    {
        public Bill()
        {
            BillDetails = new List<BillDetail>();
        }
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedOn { get; set; }
        public double DiscountPercentage { get; set; }
        public double VATPercentage { get; set; } = 5;
        public Discount Discount { get; set; }
        public List<BillDetail> BillDetails { get; set; }
        public double DiscountAmount
        {
            get
            {
                if(Discount != null && SubTotal >= Discount.PriceCriteria)
                {
                    return (DiscountPercentage / 100) * SubTotal;
                }
                return 0;
            }
        }
        public double SubTotal 
        {
            get
            {
                return BillDetails.Sum(x => (x.Price * x.Quantity));
            }
        }
        public double SubTotalWithDiscount
        {
            get
            {
                return (SubTotal - DiscountAmount);
            }
        }
        public double VATAmount
        {
            get
            {
                return (VATPercentage / 100) * SubTotal;
            }
        }
        public double TotalPrice 
        {
            get
            {
                return (SubTotalWithDiscount + VATAmount);
            }
        }
    }
}