using SQLite;

namespace FinanceTracker;

/// <summary>
/// Категория.
/// </summary>
public abstract class Category
{
    /// <summary>
    /// Ид категории.
    /// </summary>
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    /// <summary>
    /// Название категории.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Добавление категории.
    /// </summary>
    public abstract void AddCategory(string name);
    
    /// <summary>
    /// Получение категорий.
    /// </summary>
    public abstract List<Category> GetCategories();

    /// <summary>
    /// Получение категории по имени.
    /// </summary>
    /// <param name="name">Имя категории.</param>
    /// <returns></returns>
    public abstract Category GetCategoryByName(string name);
    
    /// <summary>
    /// Удаление категории.
    /// </summary>
    public abstract void RemoveCategory(int id);
    
    /// <summary>
    /// Переименование картегории.
    /// </summary>
    public abstract void RenameCategory(int id, string name);     
}