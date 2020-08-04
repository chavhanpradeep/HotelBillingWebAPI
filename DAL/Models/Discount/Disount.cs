using System;

namespace DAL.Models
{
    public class Discount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Percentage { get; set; }
        public double PriceCriteria { get; set; }
        public DateTime ApplicableFrom { get; set; }
        public DateTime ApplicableTo { get; set; }
        public bool IsRecurring { get; set; }
        public string DayOfWeek { get; set; }
    }
}