namespace FinanceTracker;

public class IncomeOperation : Operation
{
    /// <summary>
    /// Менеджер работы с БД.
    /// </summary>
    private AppDBManager database;
    
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
        operation.Sum = Sum;
        database.UpdateIncomeOperation(operation);
    }

    public override List<Operation> GetOperations()
    {
        return new List<Operation>(database.GetIncomeOperations());
    }

    public override List<Operation> GetOperationsByPeriod(DateTime startDate, DateTime endDate)
    {
        return new List<Operation>(database.GetIncomeOperationsByPeriod(startDate, endDate));;
    }

    public override List<Operation> GetTotalOperationByPeriod(DateTime startDate, DateTime endDate)
    {
        return new List<Operation>((int)database.GetTotalIncomeByPeriod(startDate, endDate));
    }

    public override List<Operation> GetOperationsByPeriodAndCategory(DateTime startDate, DateTime endDate, int categoryId)
    {
        return new List<Operation>(database.GetIncomeOperationsByPeriodAndCategory(startDate, endDate, categoryId));
    }
}