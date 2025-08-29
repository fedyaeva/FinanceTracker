namespace FinanceTracker;

/// <summary>
/// Категория прихода.
/// </summary>
public class IncomeCategory : Category
{
    /// <summary>
    /// Менеджер работы с БД.
    /// </summary>
    private AppDBManager database;
    
    public IncomeCategory(AppDBManager database)
    {
        this.database = database;
    }

    public IncomeCategory()
    {
        this.database = new AppDBManager();
    }
    
    public override void AddCategory(string name)
    {
        var existingCategory = database.GetIncomeCategories().FirstOrDefault(x => x.Name == name);
        if (existingCategory != null)
        {
            throw new Exception("Expense category already exists");
        }
        var newCategory = new IncomeCategory{Name = name};
        database.AddIncomeCategory(newCategory);
    }
    
    public override List<Category> GetCategories()
    {
        return new List<Category>(database.GetIncomeCategories());
    }
    
    public override Category GetCategoryByName(string name)
    {
        return database.GetIncomeCategories().FirstOrDefault(c => c.Name == name);
    }

    public override void RemoveCategory(int id)
    {
        var category = database.GetIncomeCategories().FirstOrDefault(x => x.Id == id);
        if (category == null)
        {
            throw new Exception("Income category not found");
        }
        database.DeleteIncomeCategory(category);
    }

    public override void RenameCategory(int id, string name)
    {
        var category = database.GetIncomeCategories().FirstOrDefault(x => x.Id == id);
        if (category == null)
        {
            throw new Exception("Income category not found");
        }
        var existingCategory = database.GetIncomeCategories().FirstOrDefault(x => x.Name == name);
        if (existingCategory != null)
        {
            throw new Exception("Income category already exists");
        }
        category.Name = name;
        database.UpdateIncomeCategory(category);
    }
}