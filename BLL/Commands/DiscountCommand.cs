using DAL.Models;
using DAL;
using System;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.DataObjects;

namespace BLL.Commands
{
    public class DiscountCommand: IDiscountCommand
    {
        public IUnitOfWork _unitOfWork;
        public DiscountCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(DiscountDTO discountDTO)
        {
            if(!DoesDiscountAlreadyExistForSameDayOfWeek(discountDTO.DayOfWeek)) 
            {
                var record = new Discount() {
                    Name = discountDTO.Name,
                    Percentage = discountDTO.Percentage,
                    Description = discountDTO.Description,
                    PriceCriteria = discountDTO.PriceCriteria,
                    ApplicableFrom = discountDTO.ApplicableFrom.Value,
                    ApplicableTo = discountDTO.ApplicableTo.Value,
                    IsRecurring = discountDTO.IsRecurring,
                    DayOfWeek = discountDTO.DayOfWeek
                };
                await _unitOfWork.Discounts.AddAsync(record);
            }
            else
                throw new Exception(
                    string.Format("Discount already exists for day: ${0}", discountDTO.DayOfWeek)
                );
        }

        public bool DoesDiscountAlreadyExistForSameDayOfWeek(string dayOfWeek)
        {
            var discount = _unitOfWork.Discounts
            .Find(x => x.DayOfWeek.ToLower() == dayOfWeek.ToLower())
            .FirstOrDefault();

            return discount != null ? true : false;
        }

        public void Update(DiscountDTO discountDTO)
        {
            var existingDiscount = _unitOfWork.Discounts.Get(discountDTO.Id);

            if(existingDiscount == null)
                throw new Exception(
                    string.Format("Could not find discount with Id {0}", discountDTO.Id)
                );

            existingDiscount.Name = discountDTO.Name;
            existingDiscount.Percentage = discountDTO.Percentage;
            existingDiscount.Description = discountDTO.Description;
            existingDiscount.PriceCriteria = discountDTO.PriceCriteria;
            existingDiscount.ApplicableFrom = discountDTO.ApplicableFrom.Value;
            existingDiscount.ApplicableTo = discountDTO.ApplicableTo.Value;
            existingDiscount.IsRecurring = discountDTO.IsRecurring;
            existingDiscount.DayOfWeek = discountDTO.DayOfWeek;

            _unitOfWork.Discounts.Update(existingDiscount);
        }

        public async Task Delete(int id)
        {
            var existingDiscount = await _unitOfWork.Discounts.GetAsync(id);

            if(existingDiscount == null)
                    throw new Exception(
                    string.Format("Could not find discount with Id {0}", id)
                );

            _unitOfWork.Discounts.Remove(existingDiscount);
        }
    }
}