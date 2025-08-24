using Android.Runtime;
using SQLite;

namespace FinanceTracker;

[Application]
public class AppDB : Application
{
    private SQLiteConnection connection;

    /// <summary>
    /// Создание таблиц.
    /// </summary>
    public AppDB()
    {
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "finance.db");
        connection = new SQLiteConnection(dbPath);
        connection.CreateTable<ExpenseCategory>();
        connection.CreateTable<IncomeCategory>();
        connection.CreateTable<ExpenseOperation>();
        connection.CreateTable<IncomeOperation>();
    }
    
    public AppDB(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
    {
    }
}