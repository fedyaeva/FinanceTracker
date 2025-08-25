namespace FinanceTracker;

public class ExpenseOperation : Operation
{
    /// <summary>
    /// Менеджер работы с БД.
    /// </summary>
    private AppDBManager database;
    
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
        operation.Sum = Sum;
        database.UpdateExpenseOperation(operation);
    }

    public override List<Operation> GetOperations()
    {
       return new List<Operation>(database.GetExpenseOperations());
    }

    public override List<Operation> GetOperationsByPeriod(DateTime startDate, DateTime endDate)
    {
        return new List<Operation>(database.GetExpenseOperationsByPeriod(startDate, endDate));
    }

    public override List<Operation> GetTotalOperationByPeriod(DateTime startDate, DateTime endDate)
    {
        return new List<Operation>((int)database.GetTotalExpenseByPeriod(startDate, endDate));
    }

    public override List<Operation> GetOperationsByPeriodAndCategory(DateTime startDate, DateTime endDate, int categoryId)
    {
        return new List<Operation>(database.GetExpenseOperationsByPeriodAndCategory(startDate, endDate, categoryId));
    }
}