namespace FinanceTracker;

/// <summary>
/// Категория.
/// </summary>
public abstract class Category
{
    /// <summary>
    /// Ид категории.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Название категории.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Добавление категории.
    /// </summary>
    public abstract void AddCategory();
    
    /// <summary>
    /// Удаление категории.
    /// </summary>
    public abstract void RemoveCategory();
    
    /// <summary>
    /// Переименование картегории.
    /// </summary>
    public abstract void RenameCategory();     
}