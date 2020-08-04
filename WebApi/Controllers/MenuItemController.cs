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
    public class MenuItemController : ControllerBase
    {

        private readonly ILogger<TestController> _logger;
        private readonly IBizLogic _bizLogic;

        public MenuItemController(ILogger<TestController> logger, IBizLogic bizLogic)
        {
            _logger = logger;
            _bizLogic = bizLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetOne(int id)
        {
            try
            {
                var item = await _bizLogic.MenuItemQuery.GetItem(id);
                return Ok(item);
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
                var items = await _bizLogic.MenuItemQuery.GetAll();
                return Ok(items);
            }
            catch(Exception)
            {
                return NotFound("Could not fetch all items");
            }
        }

        [HttpPost("new")]
        public async Task<IActionResult> NewMenuItem([FromBody] MenuItemDTO item)
        {
            try
            {
                await _bizLogic.MenuItemCommand.AddMenuItem(item);
                _bizLogic.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return NotFound("Failed to add new item");
            }
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] MenuItemDTO item)
        {
            try
            {
                _bizLogic.MenuItemCommand.Update(item);
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
                await _bizLogic.MenuItemCommand.Delete(id);
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
