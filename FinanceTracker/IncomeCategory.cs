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
    public override void AddCategory(string name)
    {
        var category = database.GetIncomeCategories().FirstOrDefault(x => x.Id == Id);
        if (category == null)
        {
            throw new Exception("Income category not found");
        }
        database.DeleteIncomeCategory(category);
    }
    
    public override List<Category> GetCategories()
    {
        return new List<Category>(database.GetIncomeCategories());
    }

    public override void RemoveCategory()
    {
        var category = database.GetIncomeCategories().FirstOrDefault(x => x.Id == Id);
        if (category == null)
        {
            throw new Exception("Income category not found");
        }
        database.DeleteIncomeCategory(category);
    }

    public override void RenameCategory()
    {
        var category = database.GetIncomeCategories().FirstOrDefault(x => x.Id == Id);
        if (category == null)
        {
            throw new Exception("Income category not found");
        }
        var existingCategory = database.GetIncomeCategories().FirstOrDefault(x => x.Name == Name);
        if (existingCategory != null)
        {
            throw new Exception("Income category already exists");
        }
        category.Name = Name;
        database.UpdateIncomeCategory(category);
    }
}