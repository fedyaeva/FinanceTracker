using Android.Content;

namespace FinanceTracker;

/// <summary>
/// Обработчик экрана добавления операции.
/// </summary>
[Activity(Label = "AddOperationActivity")]
public class AddOperationActivity : Activity
{
    private TextView textViewHeader;
    private EditText editTextAmount;
    private TextView textViewCategoryName;
    private EditText editTextComment;
    private TextView textViewSelectedDate;
    private TextView textViewSelectedTime;

    private DateTime selectedDateTime;
    
    /// <summary>
    /// Инициализация компонентов.
    /// </summary>
    /// <param name="savedInstanceState"></param>
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.add_operation); 
        
        textViewHeader = FindViewById<TextView>(Resource.Id.textViewHeader);
        editTextAmount = FindViewById<EditText>(Resource.Id.editTextAmount);
        textViewCategoryName = FindViewById<TextView>(Resource.Id.textViewCategoryName);
        editTextComment = FindViewById<EditText>(Resource.Id.editTextComment);
        textViewSelectedDate = FindViewById<TextView>(Resource.Id.textViewSelectedDate);
        textViewSelectedTime = FindViewById<TextView>(Resource.Id.textViewSelectedTime);

        string operationType = Intent.GetStringExtra("operationType") ?? "Доход";

        var layoutCategory = FindViewById<LinearLayout>(Resource.Id.layoutCategory);
        layoutCategory.Click += (s, e) =>
        {
            var intent = new Intent(this, typeof(SelectCategoryActivity));
            intent.PutExtra("operationType", operationType);
            StartActivityForResult(intent, 100);
        };

        var layoutDate = FindViewById<LinearLayout>(Resource.Id.layoutDate);
        layoutDate.Click += (s, e) => { ShowDatePicker(); };

        var layoutTime = FindViewById<LinearLayout>(Resource.Id.layoutTime);
        layoutTime.Click += (s, e) => { ShowTimePicker(); };

        var buttonBack = FindViewById<Button>(Resource.Id.buttonBack);
        buttonBack.Click += (s, e) => { Finish(); };

        var buttonSave = FindViewById<Button>(Resource.Id.buttonSave);
        buttonSave.Click += (s, e) =>
        {
            SaveOperation();
            var intent = new Intent(this, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
            StartActivity(intent);
            Finish();
        };

        selectedDateTime = DateTime.Now;
        UpdateDisplayedDateTime();
        
        textViewHeader.Text = operationType;
    }

    /// <summary>
    /// Установка выбранной категории.
    /// </summary>
    /// <param name="requestCode"></param>
    /// <param name="resultCode"></param>
    /// <param name="data"></param>
    protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
    {
        base.OnActivityResult(requestCode, resultCode, data);

        if (requestCode == 100 && resultCode == Result.Ok && data != null)
        {
            try
            {
                string categoryName = data.GetStringExtra("SelectedCategory");
                if (!string.IsNullOrEmpty(categoryName))
                {
                    textViewCategoryName.Text = categoryName;
                }
                else
                {
                    Toast.MakeText(this, "Категория не выбрана", ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Ошибка при получении категории: " + ex.Message, ToastLength.Long).Show();
            }
        }
    }
    
    /// <summary>
    /// Показать календарь выбора даты.
    /// </summary>
    private void ShowDatePicker()
    {
        var datePickerDialog = new DatePickerDialog(this,
            (sender, args) =>
            {
                selectedDateTime = new DateTime(args.Year, args.Month + 1, args.DayOfMonth,
                    selectedDateTime.Hour, selectedDateTime.Minute, 0);
                UpdateDisplayedDateTime();
            },
            selectedDateTime.Year,
            selectedDateTime.Month - 1,
            selectedDateTime.Day);

        datePickerDialog.Show();
    }
    
    /// <summary>
    /// Показать диалог выбора времени.
    /// </summary>
    private void ShowTimePicker()
    {
        var timePickerDialog = new TimePickerDialog(this,
            (sender, args) =>
            {
                selectedDateTime = new DateTime(selectedDateTime.Year,
                    selectedDateTime.Month,
                    selectedDateTime.Day,
                    args.HourOfDay,
                    args.Minute,
                    0);
                UpdateDisplayedDateTime();
            },
            selectedDateTime.Hour,
            selectedDateTime.Minute,
            true);

        timePickerDialog.Show();
    }

    /// <summary>
    /// Изменение отображения даты и времени.
    /// </summary>
    private void UpdateDisplayedDateTime()
    {
        try
        {
            textViewSelectedDate.Text = selectedDateTime.ToString("dd.MM.yyyy");
            textViewSelectedTime.Text = selectedDateTime.ToString("HH:mm");
        }
        catch (Exception ex)
        {
            Toast.MakeText(this, "Ошибка при обновлении даты: " + ex.Message, ToastLength.Long).Show();
        }
    }

    /// <summary>
    /// Сохранение операции.
    /// </summary>
    private void SaveOperation()
    {
        string amountText = editTextAmount.Text.Trim();
        string categoryName = textViewCategoryName.Text.Trim();
        string comment = editTextComment.Text.Trim();

        if (string.IsNullOrEmpty(amountText))
        {
            Toast.MakeText(this, "Пожалуйста, введите сумму", ToastLength.Short).Show();
            return;
        }

        if (!decimal.TryParse(amountText, out decimal amount))
        {
            Toast.MakeText(this, "Некорректная сумма", ToastLength.Short).Show();
            return;
        }

        if (categoryName == "[Выберите категорию]" || string.IsNullOrEmpty(categoryName))
        {
            Toast.MakeText(this, "Пожалуйста, выберите категорию", ToastLength.Short).Show();
            return;
        }

        string operationType = textViewHeader.Text;

        switch (operationType)
        {
            case "Расход":
                ExpenseCategory expenseCategory = new ExpenseCategory(new AppDBManager());
                int categoryIdExpense = expenseCategory.GetCategoryByName(categoryName).Id;
                ExpenseOperation expenseOperation = new ExpenseOperation(new AppDBManager())
                {
                    Amount = amount,
                    Comment = comment,
                    DateTime = selectedDateTime,
                    CategoryId = categoryIdExpense,
                };
                expenseOperation.AddOperation();
                break;
            case "Доход":
                IncomeCategory incomeCategory = new IncomeCategory(new AppDBManager());
                int categoryIdIncome = incomeCategory.GetCategoryByName(categoryName).Id;
                IncomeOperation incomeOperation = new IncomeOperation(new AppDBManager())
                {
                    Amount = amount,
                    Comment = comment,
                    DateTime = selectedDateTime,
                    CategoryId = categoryIdIncome,
                };
                incomeOperation.AddOperation();
                break;
        }

        Toast.MakeText(this, "Операция сохранена", ToastLength.Short).Show();
    }
}