using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BLL;
using DAL.DTOs;
using System.IO;
using IronPdf;
using DAL.Repositories.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillController : ControllerBase
    {

        private readonly ILogger<TestController> _logger;
        private IBizLogic _bizLogic;
        private IConfig _config;

        public BillController(ILogger<TestController> logger, IBizLogic bizLogic, IConfig config)
        {
            _logger = logger;
            _bizLogic = bizLogic;
            _config = config;
        }

        [HttpGet()]
        public async Task<IActionResult> GetOne(int id)
        {
            try
            {
                var bill = await _bizLogic.BillQuery.Get(id);
                return Ok(bill);
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
                var bills = await _bizLogic.BillQuery.GetAll();
                return Ok(bills);
            }
            catch(Exception)
            {
                return NotFound("Could not fetch all discounts");
            }
        }

        [HttpPost("new")]
        public async Task<IActionResult> New([FromBody] BillDTO billDTO)
        {
            try
            {
                await _bizLogic.BillCommand.Add(billDTO);
                _bizLogic.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound("Failed to add new bill");
            }
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] BillDTO billDTO)
        {
            try
            {
                //yet to implement
                return Ok();
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("generate-pdf")]
        public async Task<IActionResult> GeneratePDF(int billId)
        {
            try
            {
                var htmlContent = await _bizLogic.BillCommand.GetHTMLString(billId);
                var fileName = "bill-" + billId + ".pdf";
                var filePath = _config.SavedPDFPath + "\\" + fileName;

                var Renderer = new IronPdf.HtmlToPdf();
                Renderer.PrintOptions.CustomCssUrl = 
                Path.Combine(Directory.GetCurrentDirectory(), "style.css");

                var PDF = Renderer.RenderHtmlAsPdf(htmlContent);
                PDF.SaveAs(filePath);

                return Ok("Successfully created PDF document.");
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
