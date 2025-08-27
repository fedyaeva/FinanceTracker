namespace FinanceTracker;

public class ExpenseOperation : Operation
{
    /// <summary>
    /// Менеджер работы с БД.
    /// </summary>
    AppDBManager database;
    
    public ExpenseOperation(AppDBManager database)
    {
        this.database = database;
    }

    public ExpenseOperation()
    {
    }

    public override void AddOperation()
    {
        database.AddExpenseOperation(this);
    }
    
    public override void RemoveOperation()
    {
        var operation = database.GetExpenseOperations().FirstOrDefault(x => x.Id == Id);
        if (operation == null)
        {
            throw new Exception("Expense operation not found");
        }
        database.DeleteExpenseOperation(operation);
    }

    public override void UpdateOperation()
    {
        var operation = database.GetExpenseOperations().FirstOrDefault(x => x.Id == Id);
        if (operation == null)
        {
            throw new Exception("Expense operation not found");
        }
        operation.Name = Name;
        operation.DateTime = DateTime;
        operation.CategoryId = CategoryId;
        operation.Amount = Amount;
        database.UpdateExpenseOperation(operation);
    }

    public override List<Operation> GetOperations()
    {
       return new List<Operation>(database.GetExpenseOperations());
    }

    public override List<CategorySum> GetOperationsSumByCategoryAndPeriod(DateTime startDate, DateTime endDate)
    {
        ExpenseCategory expenseCategory = new ExpenseCategory();
        var operations = database.GetIncomeOperationsByPeriod(startDate, endDate);
        
        var categories = expenseCategory.GetCategories();
    
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
        return database.GetSumExpenseByPeriod(startDate, endDate);
    }

    public override List<Operation> GetOperationsByPeriodAndCategory(DateTime startDate, DateTime endDate, int categoryId)
    {
        return new List<Operation>(database.GetExpenseOperationsByPeriodAndCategory(startDate, endDate, categoryId));
    }
}