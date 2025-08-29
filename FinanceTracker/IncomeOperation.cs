namespace FinanceTracker;

/// <summary>
/// Операция дохода.
/// </summary>
public class IncomeOperation : Operation
{
    /// <summary>
    /// Менеджер работы с БД.
    /// </summary>
    private AppDBManager database;

    public IncomeOperation(AppDBManager database)
    {
        this.database = database;
    }

    public IncomeOperation()
    {
    }

    public override void AddOperation()
    {
        database.AddIncomeOperation(this);
    }

    public override void RemoveOperation()
    {
        var operation = database.GetIncomeOperations().FirstOrDefault(x => x.Id == Id);
        if (operation == null)
        {
            throw new Exception("Income operation not found");
        }
        database.DeleteIncomeOperation(operation);
    }

    public override void UpdateOperation()
    {
        var operation = database.GetIncomeOperations().FirstOrDefault(x => x.Id == Id);
        if (operation == null)
        {
            throw new Exception("Income operation not found");
        }
        operation.Name = Name;
        operation.DateTime = DateTime;
        operation.CategoryId = CategoryId;
        operation.Amount = Amount;
        database.UpdateIncomeOperation(operation);
    }

    public override List<Operation> GetOperations()
    {
        return new List<Operation>(database.GetIncomeOperations());
    }
    
    public override IncomeOperation GetOperation(int id)
    {
        return database.GetIncomeOperation(id);
    }

    public override List<CategorySum> GetOperationsSumByCategoryAndPeriod(DateTime startDate, DateTime endDate)
    {
        IncomeCategory incomeCategory = new IncomeCategory();
        
        var operations = database.GetIncomeOperationsByPeriod(startDate, endDate);
        var categories = incomeCategory.GetCategories();
    
        List<CategorySum> groupedSums = operations
            .GroupBy(op => op.CategoryId)
            .Select(g => new CategorySum
            {
                CategoryId = g.Key,
                CategoryName = categories.FirstOrDefault(c => c.Id == g.Key)?.Name ?? "Неизвестно",
                SumAmount = g.Sum(op => op.Amount)
            })
            .ToList();
        
        return groupedSums;        
    }

    public override decimal GetSumOperationByPeriod(DateTime startDate, DateTime endDate)
    {
        return database.GetSumIncomeByPeriod(startDate, endDate);
    }

    public override List<Operation> GetOperationsByPeriodAndCategory(DateTime startDate, DateTime endDate, int categoryId)
    {
        return new List<Operation>(database.GetIncomeOperationsByPeriodAndCategory(startDate, endDate, categoryId));
    }
}