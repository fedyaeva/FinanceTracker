namespace FinanceTracker;

public class CategorySum
{
    public int CategoryId { get; set; }
    
    public decimal SumAmount { get; set; }
    public string CategoryName { get; set; }
    
    public override string ToString()
    {
        return $"{CategoryName}: {SumAmount:F2} руб.";
    }
}