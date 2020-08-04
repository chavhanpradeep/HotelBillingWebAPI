using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using BLL.DataObjects;
using BLL.Interfaces;
using DAL;
using DAL.Models;

namespace BLL.Queries
{
    public class DiscountQuery : IDiscountQuery
    {
        private IUnitOfWork _unitOfWork;
        public DiscountQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DiscountDTO> Get(int id)
        {
            var discount = await _unitOfWork.Discounts.GetAsync(id);

            if (discount == null)
                throw new Exception(
                    string.Format("discount with id ${0} does not exist")
                );

            return new DiscountDTO()
            {
                Id = discount.Id,
                Name = discount.Name,
                Percentage = discount.Percentage,
                Description = discount.Description,
                PriceCriteria = discount.PriceCriteria,
                ApplicableFrom = discount.ApplicableFrom,
                ApplicableTo = discount.ApplicableTo,
                IsRecurring = discount.IsRecurring,
                DayOfWeek = discount.DayOfWeek
            };
        }

        public async Task<List<DiscountDTO>> GetAll()
        {
            var discountDTOs = new List<DiscountDTO>();

            var discounts = await _unitOfWork.Discounts.GetAllAsync();

            foreach (var discount in discounts.ToList())
            {
                var discountDTO = new DiscountDTO()
                {
                    Id = discount.Id,
                    Name = discount.Name,
                    Percentage = discount.Percentage,
                    Description = discount.Description,
                    PriceCriteria = discount.PriceCriteria,
                    ApplicableFrom = discount.ApplicableFrom,
                    ApplicableTo = discount.ApplicableTo,
                    IsRecurring = discount.IsRecurring,
                    DayOfWeek = discount.DayOfWeek
                };
                discountDTOs.Add(discountDTO);
            }
            return discountDTOs;
        }
    }
}