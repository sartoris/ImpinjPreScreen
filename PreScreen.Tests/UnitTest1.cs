using System.Text;
using System.IO;
using Microsoft.AspNetCore.Http;
using PreScreen.Models;
using PreScreen.Services;


namespace PreScreen.Tests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        string content = """
Region,Country,Item Type,Sales Channel,Order Priority,Order Date,Order ID,Ship Date,Units Sold,Unit Price,Unit Cost,Total Revenue,Total Cost,Total Profit
Middle East and North Africa,Azerbaijan,Snacks,Online,C,10/8/2014,535113847,10/23/2014,934,152.58,97.44,142509.72,91008.96,51500.76
""";
        IFormFile salesFile = CreateTestFormFile(content);
        List<Sales> sales = await SalesService.GetSalesFromCsvFile(salesFile);
        Assert.Single(sales);
        Assert.Equal(new DateTime(2014, 10, 8), sales[0].OrderDate);
        Assert.Equal(new DateTime(2014, 10, 23), sales[0].ShipDate);
        Assert.Equal("Azerbaijan", sales[0].Country);
        Assert.Equal((decimal)51500.76, sales[0].TotalProfit);
    }

    [Fact]
    public async Task Test2()
    {
        string content = """
Region,Country,Item Type,Sales Channel,Order Priority,Order Date,Order ID,Ship Date,Units Sold,Unit Price,Unit Cost,Total Revenue,Total Cost,Total Profit
Middle East and North Africa,Azerbaijan,Snacks,Online,C,10/8/2014,535113847,10/23/2014,934,152.58,97.44,142509.72,91008.96,51500.76
Central America and the Caribbean,Panama,Cosmetics,Offline,L,2/22/2015,874708545,2/27/2015,4551,437.20,263.33,1989697.20,1198414.83,791282.37
""";
        IFormFile salesFile = CreateTestFormFile(content);
        List<Sales> sales = await SalesService.GetSalesFromCsvFile(salesFile);
        Assert.Equal(2, sales.Count());
        Assert.Equal(new DateTime(2014, 10, 8), sales[0].OrderDate);
        Assert.Equal("Azerbaijan", sales[0].Country);
        Assert.Equal((decimal)51500.76, sales[0].TotalProfit);
        Assert.Equal(new DateTime(2015, 2, 22), sales[1].OrderDate);
        Assert.Equal("Panama", sales[1].Country);
        Assert.Equal((decimal)791282.37, sales[1].TotalProfit);
    }

    [Fact]
    public async Task TestFile()
    {
        string content = File.ReadAllText("SalesRecords.csv");;
        IFormFile salesFile = CreateTestFormFile(content);
        List<Sales> sales = await SalesService.GetSalesFromCsvFile(salesFile);
        Assert.Equal(100000, sales.Count());
        Assert.Equal(new DateTime(2014, 10, 8), sales[0].OrderDate);
        Assert.Equal("Azerbaijan", sales[0].Country);
        Assert.Equal((decimal)51500.76, sales[0].TotalProfit);
        Assert.Equal(new DateTime(2015, 2, 22), sales[1].OrderDate);
        Assert.Equal("Panama", sales[1].Country);
        Assert.Equal((decimal)791282.37, sales[1].TotalProfit);
    }

    [Fact]
    public async Task TestSummary()
    {
        string content = File.ReadAllText("SalesRecords.csv");;
        IFormFile salesFile = CreateTestFormFile(content);
        List<Sales> sales = await SalesService.GetSalesFromCsvFile(salesFile);
        var summary = new SalesSummary(sales);
        Assert.Equal((decimal)117.11, summary.MedianCost);
        Assert.Equal("Sub-Saharan Africa", summary.CommonRegion);
        Assert.Equal(new DateOnly(2010, 1, 1), summary.FirstOrder);
        Assert.Equal(new DateOnly(2017, 7, 28), summary.LastOrder);
        Assert.Equal(2765, summary.OrderDays);
        Assert.Equal((decimal)133606673066.41, summary.TotalRevenue);
    }



    private static IFormFile CreateTestFormFile(string content)
    {
        string fileName = "CsvInput";
        byte[] bytes = Encoding.UTF8.GetBytes(content);

        return new Microsoft.AspNetCore.Http.FormFile(
            baseStream: new MemoryStream(bytes),
            baseStreamOffset: 0,
            length: bytes.Length,
            name: "Data",
            fileName: fileName
        );
    }
}