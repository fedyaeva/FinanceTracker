namespace FinanceTracker;

/// <summary>
/// Обработчик экрана отображения операций по категории.
/// </summary>
[Activity(Label = "OperationByCategoryActivity")]
public class OperationByCategoryActivity : Activity
{
    private ListView listViewOperations;
    private Button buttonBack;
    private TextView textViewHeader;
    private AppDBManager database;
    private ArrayAdapter<string> adapter;

    public class Operation
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Date { get; set; }
    }

    private List<Operation> operationsList = new List<Operation>();
    private List<string> operationsStrings = new List<string>();

    private string operationType;
    private int categoryId;
    private string categoryName;

    /// <summary>
    /// Инициализация компонентов.
    /// </summary>
    /// <param name="savedInstanceState"></param>
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.operation_by_category);

        listViewOperations = FindViewById<ListView>(Resource.Id.listViewOperations);
        buttonBack = FindViewById<Button>(Resource.Id.buttonBack);
        textViewHeader = FindViewById<TextView>(Resource.Id.textViewHeader);

        operationType = Intent.GetStringExtra("OperationType") ?? "Доход";
        categoryId = Intent.GetIntExtra("CategoryId", -1);
        categoryName = Intent.GetStringExtra("CategoryName");
        textViewHeader.Text = categoryName;

        string startPeriod = Intent.GetStringExtra("StartRange");
        string endPeriod = Intent.GetStringExtra("EndRange");

        try
        {
            DateTime startDate = DateTime.Parse(startPeriod);
            DateTime endDate = DateTime.Parse(endPeriod);

            LoadOperations(startDate, endDate);
            
            operationsStrings.Clear();
            foreach (var op in operationsList)
            {
                string displayText = $"{op.Description} - {op.Amount} руб. ({op.Date})";
                operationsStrings.Add(displayText);
            }

            adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, operationsStrings);
            listViewOperations.Adapter = adapter;
            
            listViewOperations.ItemLongClick += (s, e) =>
            {
                ShowContextMenu(e.Position);
                e.Handled = true;
            };
        }
        catch (Exception ex)
        {
            Toast.MakeText(this, "Ошибка при парсинге дат: " + ex.Message, ToastLength.Long).Show();
        }

        buttonBack.Click += (s, e) => Finish();
    }

    /// <summary>
    /// Показ контекстного меню.
    /// </summary>
    /// <param name="position">Позиция.</param>
    private void ShowContextMenu(int position)
    {
        string[] options = { "Удалить" };

        var builder = new AlertDialog.Builder(this);
        builder.SetItems(options, (sender, args) =>
        {
            int which = args.Which;
            if (which == 0)
                ConfirmAndDeleteOperation(position);
        });
        builder.Show();
    }
    
    /// <summary>
    /// Подтверждение и удаление операции.
    /// </summary>
    /// <param name="position"></param>
    private void ConfirmAndDeleteOperation(int position)
    {
        var operation = operationsList[position];

        var alertBuilder = new AlertDialog.Builder(this);
        alertBuilder.SetMessage($"Вы уверены, что хотите удалить операцию '{operation.Description}'?");
        alertBuilder.SetPositiveButton("Да", (senderAlert, args) =>
        {
            try
            {
                bool success = DeleteOperationFromDB(operation.Id);
                if (success)
                {
                    operationsList.RemoveAt(position);
                    operationsStrings.RemoveAt(position);
                    adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, operationsStrings);
                    listViewOperations.Adapter = adapter;
                    (listViewOperations.Adapter as ArrayAdapter<string>)?.NotifyDataSetChanged();
                    adapter.NotifyDataSetChanged();
                    Toast.MakeText(this, "Операция удалена", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this, "Ошибка при удалении операции", ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Ошибка: " + ex.Message, ToastLength.Long).Show();
            }
        });
        alertBuilder.SetNegativeButton("Нет", (sender, args) => { });
        alertBuilder.Show();
    }

    /// <summary>
    /// Удаление операции из БД.
    /// </summary>
    /// <param name="operationId"></param>
    /// <returns></returns>
    private bool DeleteOperationFromDB(int operationId)
    {
        try
        {
            database = new AppDBManager();

            if (operationType == "Доход")
            {
                var incomeOp = database.GetIncomeOperation(operationId);
                if (incomeOp != null)
                {
                    database.DeleteIncomeOperation(incomeOp);
                    return true;
                }
            }
            else
            {
                var expenseOp = database.GetExpenseOperation(operationId);
                if (expenseOp != null)
                {
                    database.DeleteExpenseOperation(expenseOp);
                    return true;
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Загрузка операций.
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    private void LoadOperations(DateTime startDate, DateTime endDate)
    {
        database = new AppDBManager();

        try
        {
            if (operationType == "Доход")
            {
                IncomeOperation incomeOperation = new IncomeOperation(database);
                var incomeOperations = incomeOperation.GetOperationsByPeriodAndCategory(startDate, endDate, categoryId);

                operationsList = incomeOperations.Select(op => new Operation
                {
                    Id = op.Id,
                    Description = op.Comment,
                    Amount = op.Amount,
                    Date = op.DateTime.ToString("yyyy-MM-dd")
                }).ToList();
            }
            else
            {
                ExpenseOperation expenseOperation = new ExpenseOperation(database);
                var expenseOperations = expenseOperation.GetOperationsByPeriodAndCategory(startDate, endDate, categoryId);

                operationsList = expenseOperations.Select(op => new Operation
                {
                    Id = op.Id,
                    Description = op.Comment,
                    Amount = op.Amount,
                    Date = op.DateTime.ToString("yyyy-MM-dd")
                }).ToList();
            }
        }
        catch (Exception ex)
        {
            Toast.MakeText(this, "Ошибка при загрузке операций: " + ex.Message, ToastLength.Long).Show();
        }
    }
}