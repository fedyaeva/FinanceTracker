using SQLite;

namespace FinanceTracker;

public class AppDB
{ 
    private SQLiteAsyncConnection connection;

    /// <summary>
    /// Создание таблиц.
    /// </summary>
    public AppDB()
    {
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "finance.db");
        connection = new SQLiteAsyncConnection(dbPath);
        connection.CreateTableAsync<ExpenseCategory>();
        connection.CreateTableAsync<IncomeCategory>();
        connection.CreateTableAsync<ExpenseOperation>();
        connection.CreateTableAsync<IncomeOperation>();
    }

    /// <summary>
    /// Добавление категории расхода.
    /// </summary>
    /// <param name="expenseCategory">Категория расхода.</param>
    public void AddExpenseCategory(ExpenseCategory expenseCategory)
    {
        connection.InsertAsync(expenseCategory);
    }

    /// <summary>
    /// Получение списка категорий расхода.
    /// </summary>
    /// <returns></returns>
    public Task<List<ExpenseCategory>> GetExpenseCategories()
    {
        return connection.Table<ExpenseCategory>().ToListAsync();
    }

    /// <summary>
    /// Изменение категории расхода.
    /// </summary>
    /// <param name="expenseCategory">Категория расхода.</param>
    public void UpdateExpenseCategory(ExpenseCategory expenseCategory)
    {
        connection.UpdateAsync(expenseCategory);
    }

    /// <summary>
    /// Удаление категории расхода.
    /// </summary>
    /// <param name="expenseCategory">Категория расхода.</param>
    public void DeleteExpenseCategory(ExpenseCategory expenseCategory)
    {
        connection.DeleteAsync(expenseCategory);
    }
    
    /// <summary>
    /// Добавление категории дохода.
    /// </summary>
    /// <param name="incomeCategory">Категория дохода.</param>
    public void IncomeCategory(IncomeCategory incomeCategory)
    {
        connection.InsertAsync(incomeCategory);
    }
    
    /// <summary>
    /// Получение списка категорий дохода.
    /// </summary>
    /// <returns></returns>
    public Task<List<IncomeCategory>> GetIncomeCategories()
    {
        return connection.Table<IncomeCategory>().ToListAsync();
    }
    
    /// <summary>
    /// Изменение категории дохода.
    /// </summary>
    /// <param name="incomeCategory">Категория дохода.</param>
    public void UpdateIncomeCategory(IncomeCategory incomeCategory)
    {
        connection.UpdateAsync(incomeCategory);
    }
    
    /// <summary>
    /// Удаление категории дохода.
    /// </summary>
    /// <param name="incomeCategory"></param>
    public void DeleteIncomeCategory(IncomeCategory incomeCategory)
    {
        connection.DeleteAsync(incomeCategory);
    }
    
    /// <summary>
    /// Добавление операции расхода.
    /// </summary>
    /// <param name="expenseOperation">Операция расхода.</param>
    public void AddExpenseOperation(ExpenseOperation expenseOperation)
    {
        connection.InsertAsync(expenseOperation);
    }

    /// <summary>
    /// Получение списка операций расхода.
    /// </summary>
    /// <returns></returns>
    public Task<List<ExpenseOperation>> GetExpenseOperation()
    {
        return connection.Table<ExpenseOperation>().ToListAsync();
    }

    /// <summary>
    /// Изменение операции расхода.
    /// </summary>
    /// <param name="expenseOperation">Операция расхода.</param>
    public void UpdateExpenseOperation(ExpenseOperation expenseOperation)
    {
        connection.UpdateAsync(expenseOperation);
    }

    /// <summary>
    /// Удаление операции расхода.
    /// </summary>
    /// <param name="expenseOperation">Операция расхода.</param>
    public void DeleteExpenseOperation(ExpenseOperation expenseOperation)
    {
        connection.DeleteAsync(expenseOperation);
    }
    
    /// <summary>
    /// Добавление операции дохода.
    /// </summary>
    /// <param name="incomeOperation">Операция дохода.</param>
    public void IncomeOperation(IncomeOperation incomeOperation)
    {
        connection.InsertAsync(incomeOperation);
    }
    
    /// <summary>
    /// Получение списка операций дохода.
    /// </summary>
    /// <returns></returns>
    public Task<List<IncomeOperation>> GetIncomeOperation()
    {
        return connection.Table<IncomeOperation>().ToListAsync();
    }
    
    /// <summary>
    /// Изменение операции дохода.
    /// </summary>
    /// <param name="incomeOperation">Операция дохода.</param>
    public void UpdateIncomeOperation(IncomeOperation incomeOperation)
    {
        connection.UpdateAsync(incomeOperation);
    }
    
    /// <summary>
    /// Удаление операции дохода.
    /// </summary>
    /// <param name="incomeOperation"></param>
    public void DeleteIncomeOperation(IncomeOperation incomeOperation)
    {
        connection.DeleteAsync(incomeOperation);
    }
}

