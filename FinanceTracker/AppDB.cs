using SQLite;
namespace FinanceTracker;

/// <summary>
/// Инициализацция БД.
/// </summary>
public class AppDB
{
    private SQLiteConnection connection;
    
    /// <summary>
    /// Создание таблиц.
    /// </summary>
    public void Initialize()
    {
        string dbPath = Android.App.Application.Context.GetDatabasePath("finance.db").AbsolutePath;
        
        connection = new SQLiteConnection(dbPath);
        
        connection.CreateTable<ExpenseCategory>();
        
        connection.CreateTable<IncomeCategory>();
        
        connection.CreateTable<ExpenseOperation>();
        
        connection.CreateTable<IncomeOperation>();
        
        var existingExpenseCategory = connection.Table<ExpenseCategory>()
            .FirstOrDefault(c => c.Name == "Продукты");
        
        if (existingExpenseCategory == null)
        {
            var newExpenseCategory = new ExpenseCategory
            {
                Name = "Продукты"
            };
            connection.Insert(newExpenseCategory);
        }
        
        var existingIncomeCategory = connection.Table<IncomeCategory>()
            .FirstOrDefault(c => c.Name == "Зарплата");
        
        if (existingIncomeCategory == null)
        {
            var newIncomeCategory = new IncomeCategory
            {
                Name = "Зарплата"
            };
            connection.Insert(newIncomeCategory);
        }
    }
    
    public AppDB()
    {
    }
}