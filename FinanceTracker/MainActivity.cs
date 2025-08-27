using Android.Content;
using SQLite;

namespace FinanceTracker;

[Activity(Label = "@string/app_name", MainLauncher = true)]
public class MainActivity : Activity
{
    private AppDB database;

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        
        SetContentView(Resource.Layout.activity_main);
        
        database = new AppDB();
        
        database.Initialize();
        
        var buttonAddExpense = FindViewById<Button>(Resource.Id.buttonAddExpense);
        var buttonAllExpenses = FindViewById<Button>(Resource.Id.buttonAllExpenses);
        var buttonAddIncome = FindViewById<Button>(Resource.Id.buttonAddIncome);
        var buttonAllIncome = FindViewById<Button>(Resource.Id.buttonAllIncome);
        
        buttonAddExpense.Click += (s, e) =>
        {
           var intent = new Intent(this, typeof(AddOperationActivity));
           intent.PutExtra("operationType", "expense");
           StartActivity(intent);
        };

        buttonAllExpenses.Click += (s, e) =>
        {
           var intent = new Intent(this, typeof(OperationSumByPeriodActivity));
           intent.PutExtra("operationType", "expense");
           StartActivity(intent);
        };

        buttonAddIncome.Click += (s, e) =>
        {
           var intent = new Intent(this, typeof(AddOperationActivity));
           intent.PutExtra("operationType", "income");
           StartActivity(intent);
        };
        
        buttonAllIncome.Click += (s, e) =>
        {
           var intent = new Intent(this, typeof(OperationSumByPeriodActivity));
           intent.PutExtra("operationType", "income");
           StartActivity(intent);
       };
        
       DisplaySumOperationsByPeriod();
    }
    
    /// <summary>
    /// Отображение сумм операций за период.
    /// </summary>
    private void DisplaySumOperationsByPeriod()
    {
       AppDBManager database = new AppDBManager();
       ExpenseOperation expenseOperation = new ExpenseOperation(database);
       IncomeOperation incomeOperation = new IncomeOperation(database);
       
       DateTime today = DateTime.Today;
       
       DateTime dayStart = today;
       DateTime dayEnd = today.AddDays(1).AddTicks(-1);
       
       int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
       DateTime weekStart = today.AddDays(-diff);
       DateTime weekEnd = weekStart.AddDays(6).AddDays(1).AddTicks(-1);
       
       DateTime monthStart = new DateTime(today.Year, today.Month, 1);
       DateTime monthEnd = monthStart.AddMonths(1).AddTicks(-1);
       
       decimal expensesDaySum = 0;
       decimal expensesWeekSum = 0;
       decimal expensesMonthSum = 0;

       decimal incomeDaySum = 0;
       decimal incomeWeekSum = 0;
       decimal incomeMonthSum = 0;
       
       expensesDaySum = expenseOperation.GetSumOperationByPeriod(dayStart, dayEnd);
       expensesWeekSum = expenseOperation.GetSumOperationByPeriod(weekStart, weekEnd);
       expensesMonthSum = expenseOperation.GetSumOperationByPeriod(monthStart, monthEnd);

       incomeDaySum = incomeOperation.GetSumOperationByPeriod(dayStart, dayEnd);
       incomeWeekSum = incomeOperation.GetSumOperationByPeriod(weekStart, weekEnd);
       incomeMonthSum = incomeOperation.GetSumOperationByPeriod(monthStart, monthEnd);
       
       FindViewById<TextView>(Resource.Id.expensesDayValue).Text = $"{expensesDaySum} руб.";
       FindViewById<TextView>(Resource.Id.expensesWeekValue).Text = $"{expensesWeekSum} руб.";
       FindViewById<TextView>(Resource.Id.expensesMonthValue).Text = $"{expensesMonthSum} руб.";

       FindViewById<TextView>(Resource.Id.incomeDayValue).Text = $"{incomeDaySum} руб.";
       FindViewById<TextView>(Resource.Id.incomeWeekValue).Text = $"{incomeWeekSum} руб.";
       FindViewById<TextView>(Resource.Id.incomeMonthValue).Text = $"{incomeMonthSum} руб.";
    }
}