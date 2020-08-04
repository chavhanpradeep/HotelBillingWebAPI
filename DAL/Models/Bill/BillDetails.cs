using System;

namespace DAL.Models
{
    public class BillDetail
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public Bill Bill { get; set; }
    }
}