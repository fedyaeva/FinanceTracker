using Android.Content;

namespace FinanceTracker;

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

        var layoutCategory = FindViewById<LinearLayout>(Resource.Id.layoutCategory);
        layoutCategory.Click += (s, e) =>
        {
            var intent = new Intent(this, typeof(SelectCategoryActivity));
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

        string operationType = Intent.GetStringExtra("OperationType") ?? "Доход";
        textViewHeader.Text = operationType;
    }

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
    {
        base.OnActivityResult(requestCode, resultCode, data);

        if (requestCode == 100 && resultCode == Result.Ok && data != null)
        {
            string categoryName = data.GetStringExtra("SelectedCategory");
            if (!string.IsNullOrEmpty(categoryName))
            {
                textViewCategoryName.Text = categoryName;
            }
        }
    }

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

    private void UpdateDisplayedDateTime()
    {
        textViewSelectedDate.Text = selectedDateTime.ToString("dd.MM.yyyy");
        textViewSelectedTime.Text = selectedDateTime.ToString("HH:mm");
    }

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
                ExpenseOperation expenseOperation = new ExpenseOperation(new AppDBManager())
                {
                    Amount = amount,
                   // Category = categoryName,
                    Comment = comment,
                    DateTime = selectedDateTime,
                };
                expenseOperation.AddOperation();
                break;
            case "Доход":
                IncomeOperation incomeOperation = new IncomeOperation(new AppDBManager())
                {
                    Amount = amount,
                  //  Category = categoryName,
                    Comment = comment,
                    DateTime = selectedDateTime,
                };
                incomeOperation.AddOperation();
                break;
        }

        Toast.MakeText(this, "Операция сохранена", ToastLength.Short).Show();
    }
}