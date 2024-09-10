using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreScreen.Models;
using PreScreen.Services;

namespace PreScreen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {

        [HttpPost("fromcsv")]
        public async Task<IActionResult> SalesFromCsv(IFormFile salesFile)
        {
            if (salesFile is null)
                throw new Exception("Unable to view sales because POST body converted to null. Check properties.");

            List<Sales> sales = await SalesService.GetSalesFromCsvFile(salesFile);
            if (!sales.Any())
                throw new BadHttpRequestException("Unable to view sales because no sales were provided in the input file.");

			return Ok(sales);
        }

        [HttpPost("fromcsv/summary")]
        public async Task<IActionResult> SummarySalesFromCsv(IFormFile salesFile)
        {
            if (salesFile is null)
                throw new Exception("Unable to view sales because POST body converted to null. Check properties.");

            List<Sales> sales = await SalesService.GetSalesFromCsvFile(salesFile);
            if (!sales.Any())
                throw new BadHttpRequestException("Unable to view sales because no sales were provided in the input file.");

			return Ok(new SalesSummary(sales));
        }



    }
}
