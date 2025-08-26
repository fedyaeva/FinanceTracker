using SQLite;
namespace FinanceTracker;

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
    }
    
    public AppDB()
    {
    }
}