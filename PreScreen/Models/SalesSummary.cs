namespace PreScreen.Models;

public class SalesSummary
{
    public SalesSummary(List<Sales> sales)
    {
        MedianCost = Median(sales.Select(x => x.UnitCost));
        CommonRegion = Mode(sales.Select(x => x.Region));
        FirstOrder = First(sales.Select(x => x.OrderDate));
        LastOrder = Last(sales.Select(x => x.OrderDate));
        OrderDays = LastOrder.DayNumber - FirstOrder.DayNumber;
        TotalRevenue = sales.Sum(x => x.TotalRevenue);
    }

    public decimal MedianCost {get;}
    public string CommonRegion {get;}
    public DateOnly FirstOrder {get;}
    public DateOnly LastOrder {get;}
    public int OrderDays {get;}
    public decimal TotalRevenue {get;}

    private decimal Median(IEnumerable<decimal> numbers)
    {
        decimal median;
        int count = numbers.Count();
        if (count % 2 == 0)
            median = numbers.OrderBy(x => x).Skip((count / 2) - 1).Take(2).Average();
        else
            median = numbers.OrderBy(x => x).ElementAt(count / 2);        
        return median;
    }

    private string Mode(IEnumerable<string> stringList)
    {
        return stringList.GroupBy(v => v)
            .OrderByDescending(g => g.Count())
            .First()
            .Key;
    }

    private DateOnly First(IEnumerable<DateTime> dateList)
    {
        DateTime firstDate = dateList.OrderBy(d => d).First();
        return DateOnly.FromDateTime(firstDate);
    }

    private DateOnly Last(IEnumerable<DateTime> dateList)
    {
        DateTime lastDate = dateList.OrderByDescending(d => d).First();
        return DateOnly.FromDateTime(lastDate);
    }

}
