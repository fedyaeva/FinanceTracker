namespace FinanceTracker;

/// <summary>
/// Категория расхода.
/// </summary>
public class ExpenseCategory : Category
{
    /// <summary>
    /// Менеджер работы с БД.
    /// </summary>
    private AppDBManager database;
    
    public ExpenseCategory(AppDBManager database)
    {
        this.database = database;
    }

    public ExpenseCategory()
    {
    }
    
    public override void AddCategory(string name)
    {
        var existingCategory = database.GetExpenseCategories().FirstOrDefault(x => x.Name == name);
        if (existingCategory != null)
        {
            throw new Exception("Expense category already exists");
        }
        var newCategory = new ExpenseCategory{Name = name};
        database.AddExpenseCategory(newCategory);
    }

    public override List<Category> GetCategories()
    {
        return new List<Category>(database.GetExpenseCategories());
    }

    public override void RemoveCategory(int id)
    {
        var category = database.GetExpenseCategories().FirstOrDefault(x => x.Id == Id);
        if (category == null)
        {
            throw new Exception("Expense category not found");
        }
        database.DeleteExpenseCategory(category);
    }

    public override void RenameCategory()
    {
        var category = database.GetExpenseCategories().FirstOrDefault(x => x.Id == Id);
        if (category == null)
        {
            throw new Exception("Expense category not found");
        }
        var existingCategory = database.GetExpenseCategories().FirstOrDefault(x => x.Name == Name);
        if (existingCategory != null)
        {
            throw new Exception("Expense category already exists");
        }
        category.Name = Name;
        database.UpdateExpenseCategory(category);
    }
}