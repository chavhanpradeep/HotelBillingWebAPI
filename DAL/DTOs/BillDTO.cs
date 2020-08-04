using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.DTOs
{
    public class BillDTO
    {
        public BillDTO()
        {
            BillDetails = new List<BillDetailDTO>();
        }
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime BillGeneratedOn { get; set; }
        public List<BillDetailDTO> BillDetails { get; set; }
        public double? SubTotal { get; set; }
    }
}