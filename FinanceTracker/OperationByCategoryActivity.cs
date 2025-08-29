namespace FinanceTracker;

[Activity(Label = "OperationByCategoryActivity")]
public class OperationByCategoryActivity : Activity
{
    private ListView listViewOperations;
    private Button buttonBack;
    private TextView textViewHeader;
    private AppDBManager database;
    
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
        
        string startRangeStr = Intent.GetStringExtra("StartRange");
        string endRangeStr = Intent.GetStringExtra("EndRange");
        
        try
        {
            DateTime startDate = DateTime.Parse(startRangeStr);
            DateTime endDate = DateTime.Parse(endRangeStr);
            
            LoadOperations(startDate, endDate);
            
            foreach (var op in operationsList)
            {
                string displayText = $"{op.Description} - {op.Amount} руб. ({op.Date})";
                operationsStrings.Add(displayText);
            }
            
            listViewOperations.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, operationsStrings);
        }
        catch (Exception ex)
        {
            Toast.MakeText(this, "Ошибка при парсинге дат: " + ex.Message, ToastLength.Long).Show();
        }
        
        buttonBack.Click += (s, e) =>
        {
            Finish();
        };
    }

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