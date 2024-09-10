using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using PreScreen.Models;

namespace PreScreen.Services;

public static class SalesService
{

	public static async Task<List<Sales>> GetSalesFromCsvFile(IFormFile salesFile)
	{
        var sales = new List<Sales>();
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true
        };
        await using (var stream = salesFile.OpenReadStream())
        {
            using (var reader = new StreamReader(stream))
            {
                using var csv = new CsvReader(reader, config);
                sales = await csv.GetRecordsAsync<Sales>().ToListAsync();
            }
        }

        return sales;
	}
	
}
