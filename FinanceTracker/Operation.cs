using SQLite;
namespace FinanceTracker;

/// <summary>
/// Операция.
/// </summary>
public abstract class Operation
{
    /// <summary>
    /// Ид операции.
    /// </summary>
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    /// <summary>
    /// Наименование операции.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Дата и время операции.
    /// </summary>
    public DateTime DateTime { get; set; }
    
    /// <summary>
    /// Ид категории.
    /// </summary>
    public int CategoryId { get; set; }
    
    /// <summary>
    /// Сумма операции.
    /// </summary>
    public int Sum { get; set; }
    
    /// <summary>
    /// Комментарий.
    /// </summary>
    public string Comment { get; set; }
   
    /// <summary>
    /// Добавление операции.
    /// </summary>
    public abstract void AddOperation();
   
    /// <summary>
    /// Удаление операции.
    /// </summary>
    public abstract void RemoveOperation();
   
    /// <summary>
    /// Редактирование операции.
    /// </summary>
    public abstract void UpdateOperation();

    /// <summary>
    /// Получение списка операций.
    /// </summary>
    public abstract List<Operation> GetOperations();
    
    /// <summary>
    /// Получение списка операций за период.
    /// </summary>
    /// <returns></returns>
    public abstract List<Operation> GetOperationsByPeriod(DateTime startDate, DateTime endDate);
    
    /// <summary>
    /// Получение общей суммы операций за период.
    /// </summary>
    /// <returns></returns>
    public abstract List<Operation> GetTotalOperationByPeriod(DateTime startDate, DateTime endDate);
    
    /// <summary>
    /// Получение списка операций за период по категории.
    /// </summary>
    /// <returns></returns>
    public abstract List<Operation> GetOperationsByPeriodAndCategory(DateTime startDate, DateTime endDate,
        int categoryId);
}