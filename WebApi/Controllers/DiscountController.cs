using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BLL;
using DAL.Models;
using BLL.DataObjects;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountController : ControllerBase
    {

        private readonly ILogger<TestController> _logger;
        private readonly IBizLogic _bizLogic;

        public DiscountController(ILogger<TestController> logger, IBizLogic bizLogic)
        {
            _logger = logger;
            _bizLogic = bizLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetOne(int id)
        {
            try
            {
                var discount = await _bizLogic.DiscountQuery.Get(id);
                return Ok(discount);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var discounts = await _bizLogic.DiscountQuery.GetAll();
                return Ok(discounts);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound("Could not fetch all discounts");
            }
        }

        [HttpPost("new")]
        public IActionResult NewMenuItem([FromBody] DiscountDTO discountDTO)
        {
            try
            {
                _bizLogic.DiscountCommand.Add(discountDTO);
                _bizLogic.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound("Failed to add new discount");
            }
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] DiscountDTO discountDTO)
        {
            try
            {
                _bizLogic.DiscountCommand.Update(discountDTO);
                _bizLogic.SaveChanges();
                return Ok();
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // [HttpPut("update-many")]
        // public IActionResult UpdateMany([FromBody] List<MenuItem> items)
        // {
        //     try
        //     {
        //         _bizLogic.MenuItemCommand.UpdateMany(items);
        //         _bizLogic.SaveChanges();
        //         return Ok();
        //     }
        //     catch(Exception ex)
        //     {
        //         return NotFound(ex.Message);
        //     }
        // }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteOne([FromQuery] int id)
        {
            try
            {
                await _bizLogic.DiscountCommand.Delete(id);
                _bizLogic.SaveChanges();
                return Ok();
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("delete-many")]
        public IActionResult DeleteMany([FromBody] List<int> ids)
        {
            //yet to implement
            return Ok();
        }
    }
}
