using SQLite;

namespace FinanceTracker;

public class AppDBManager
{ 
    private SQLiteConnection connection;

    /// <summary>
    /// Добавление категории расхода.
    /// </summary>
    /// <param name="expenseCategory">Категория расхода.</param>
    public void AddExpenseCategory(ExpenseCategory expenseCategory)
    {
        connection.Insert(expenseCategory);
    }

    /// <summary>
    /// Получение списка категорий расхода.
    /// </summary>
    /// <returns></returns>
    public List<ExpenseCategory> GetExpenseCategories()
    {
        return connection.Table<ExpenseCategory>().ToList();
    }

    /// <summary>
    /// Изменение категории расхода.
    /// </summary>
    /// <param name="expenseCategory">Категория расхода.</param>
    public void UpdateExpenseCategory(ExpenseCategory expenseCategory)
    {
        connection.Update(expenseCategory);
    }

    /// <summary>
    /// Удаление категории расхода.
    /// </summary>
    /// <param name="expenseCategory">Категория расхода.</param>
    public void DeleteExpenseCategory(ExpenseCategory expenseCategory)
    {
        connection.Delete(expenseCategory);
    }
    
    /// <summary>
    /// Добавление категории дохода.
    /// </summary>
    /// <param name="incomeCategory">Категория дохода.</param>
    public void AddIncomeCategory(IncomeCategory incomeCategory)
    {
        connection.Insert(incomeCategory);
    }
    
    /// <summary>
    /// Получение списка категорий дохода.
    /// </summary>
    /// <returns></returns>
    public List<IncomeCategory> GetIncomeCategories()
    {
        return connection.Table<IncomeCategory>().ToList();
    }
    
    /// <summary>
    /// Изменение категории дохода.
    /// </summary>
    /// <param name="incomeCategory">Категория дохода.</param>
    public void UpdateIncomeCategory(IncomeCategory incomeCategory)
    {
        connection.Update(incomeCategory);
    }
    
    /// <summary>
    /// Удаление категории дохода.
    /// </summary>
    /// <param name="incomeCategory"></param>
    public void DeleteIncomeCategory(IncomeCategory incomeCategory)
    {
        connection.Delete(incomeCategory);
    }
    
    /// <summary>
    /// Добавление операции расхода.
    /// </summary>
    /// <param name="expenseOperation">Операция расхода.</param>
    public void AddExpenseOperation(ExpenseOperation expenseOperation)
    {
        connection.Insert(expenseOperation);
    }

    /// <summary>
    /// Получение списка операций расхода.
    /// </summary>
    /// <returns></returns>
    public List<ExpenseOperation> GetExpenseOperations()
    {
       return connection.Table<ExpenseOperation>().ToList();
    }

    /// <summary>
    /// Получение списка операций расхода за период.
    /// </summary>
    /// <param name="startDate">Начало периода.</param>
    /// <param name="endDate">Конец периода.</param>
    /// <returns></returns>
    public List<ExpenseOperation> GetExpenseOperationsByPeriod(DateTime startDate, DateTime endDate)
    {
        return connection.Table<ExpenseOperation>().Where(o =>
            o.DateTime >= startDate && 
            o.DateTime <= endDate)
            .ToList();
    }
    
    /// <summary>
    /// Получение суммы расходов за период.
    /// </summary>
    /// <param name="startDate">Начало периода.</param>
    /// <param name="endDate">Конец периода.</param>
    /// <returns></returns>
    public decimal GetTotalExpenseByPeriod(DateTime startDate, DateTime endDate)
    {
        return connection.Table<ExpenseOperation>().Where(o => 
                o.DateTime >= startDate && 
                o.DateTime <= endDate)
            .Sum(o => o.Sum);
    }
    
    /// <summary>
    /// Получение списка операций расхода за период по категории.
    /// </summary>
    /// <param name="startDate">Начало периода.</param>
    /// <param name="endDate">Конец периода.</param>
    /// <param name="expenseCategory">ИД категории расхода.</param>
    /// <returns></returns>
    public List<ExpenseOperation> GetExpenseOperationsByPeriodAndCategory(DateTime startDate, DateTime endDate,
        int expenseCategory)
    {
        return connection.Table<ExpenseOperation>().Where(o =>
                o.DateTime >= startDate && 
                o.DateTime <= endDate && 
                o.CategoryId == expenseCategory)
            .ToList();
    }

    /// <summary>
    /// Изменение операции расхода.
    /// </summary>
    /// <param name="expenseOperation">Операция расхода.</param>
    public void UpdateExpenseOperation(ExpenseOperation expenseOperation)
    {
        connection.Update(expenseOperation);
    }

    /// <summary>
    /// Удаление операции расхода.
    /// </summary>
    /// <param name="expenseOperation">Операция расхода.</param>
    public void DeleteExpenseOperation(ExpenseOperation expenseOperation)
    {
        connection.Delete(expenseOperation);
    }
    
    /// <summary>
    /// Добавление операции дохода.
    /// </summary>
    /// <param name="incomeOperation">Операция дохода.</param>
    public void AddIncomeOperation(IncomeOperation incomeOperation)
    {
        connection.Insert(incomeOperation);
    }
    
    /// <summary>
    /// Получение списка операций дохода.
    /// </summary>
    /// <returns></returns>
    public List<IncomeOperation> GetIncomeOperations()
    {
        return connection.Table<IncomeOperation>().ToList();
    }
    
    /// <summary>
    /// Получение списка операцй дохода за период.
    /// </summary>
    /// <param name="startDate">Начало периода.</param>
    /// <param name="endDate">Конец периода.</param>
    /// <returns></returns>
    public List<IncomeOperation> GetIncomeOperationsByPeriod(DateTime startDate, DateTime endDate)
    {
        return connection.Table<IncomeOperation>().Where(o =>
            o.DateTime >= startDate && 
            o.DateTime <= endDate)
            .ToList();
    }

    /// <summary>
    /// Получение списка операций дохода за период по категории.
    /// </summary>
    /// <param name="startDate">Начало периода.</param>
    /// <param name="endDate">Конец периода.</param>
    /// <param name="expenseCategory">ИД категории дохода.</param>
    /// <returns></returns>
    public List<IncomeOperation> GetIncomeOperationsByPeriodAndCategory(DateTime startDate, DateTime endDate,
        int incomeCategory)
    {
        return connection.Table<IncomeOperation>().Where(o =>
            o.DateTime >= startDate && 
            o.DateTime <= endDate && 
            o.CategoryId == incomeCategory)
            .ToList();
    }
    
    /// <summary>
    /// Получение суммы доходов за период.
    /// </summary>
    /// <param name="startDate">Начало периода.</param>
    /// <param name="endDate">Конец периода.</param>
    /// <returns></returns>
    public decimal GetTotalIncomeByPeriod(DateTime startDate, DateTime endDate)
    {
        return connection.Table<IncomeOperation>().Where(o => 
                o.DateTime >= startDate && 
                o.DateTime <= endDate)
            .Sum(o => o.Sum);
    }
    
    /// <summary>
    /// Изменение операции дохода.
    /// </summary>
    /// <param name="incomeOperation">Операция дохода.</param>
    public void UpdateIncomeOperation(IncomeOperation incomeOperation)
    {
        connection.Update(incomeOperation);
    }
    
    /// <summary>
    /// Удаление операции дохода.
    /// </summary>
    /// <param name="incomeOperation"></param>
    public void DeleteIncomeOperation(IncomeOperation incomeOperation)
    {
        connection.Delete(incomeOperation);
    }
}

