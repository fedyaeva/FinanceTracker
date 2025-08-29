using Android.Content;
namespace FinanceTracker; 

/// <summary>
/// Обработчик экрана отображения сумм операций по категориям.
/// </summary>
[Activity(Label = "OperationSumByPeriodActivity")]
public class OperationSumByPeriodActivity : Activity
{
        private Button buttonBack;
        private Button buttonPrev;
        private Button buttonNext;
        private TextView textViewPeriodTitle;
        private TextView textViewCurrentPeriodDisplay;
        private ListView listViewCategoriesSum;
        private string operationType;
        private AppDBManager database;
        private ArrayAdapter<CategorySum> adapter;
        private DateTime startRange;
        private DateTime endRange;

        enum PeriodType
        {
            Day,
            Week,
            Month,
            Year,
            AllTime
        }

        private PeriodType currentPeriodType = PeriodType.Month;
        private DateTime currentDate;

        private List<CategorySum> categorySums = new List<CategorySum>();
        
        /// <summary>
        /// Инициализация компонентов.
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.operation_sum_by_period);
            
            database = new AppDBManager();

            buttonBack = FindViewById<Button>(Resource.Id.buttonBack);
            buttonPrev = FindViewById<Button>(Resource.Id.buttonPrevPeriod);
            buttonNext = FindViewById<Button>(Resource.Id.buttonNextPeriod);
            textViewPeriodTitle = FindViewById<TextView>(Resource.Id.textViewPeriodTitle);
            textViewCurrentPeriodDisplay = FindViewById<TextView>(Resource.Id.textViewCurrentPeriodDisplay);
            listViewCategoriesSum = FindViewById<ListView>(Resource.Id.listViewCategoriesSum);
            
            operationType = Intent.GetStringExtra("operationType") ?? "Доход";

            currentDate = DateTime.Now;
            
            adapter = new ArrayAdapter<CategorySum>(this, Android.Resource.Layout.SimpleListItem1, categorySums);

            UpdatePeriodDisplay();
            LoadAndDisplaySumByPeriod();

            buttonBack.Click += (s, e) => Finish();

            buttonPrev.Click += (s, e) =>
            {
                ChangePeriod(-1);
                UpdatePeriodDisplay();
                LoadAndDisplaySumByPeriod();
            };

            buttonNext.Click += (s, e) =>
            {
                ChangePeriod(1);
                UpdatePeriodDisplay();
                LoadAndDisplaySumByPeriod();
            };

            textViewPeriodTitle.Click += (s, e) => ShowPeriodTypeDialog();

          adapter = new ArrayAdapter<CategorySum>(this, Android.Resource.Layout.SimpleListItem1, categorySums);
           listViewCategoriesSum.Adapter = adapter;

            listViewCategoriesSum.ItemClick += (s, e) =>
            {
                var categorySum = categorySums[e.Position];
                var intent = new Intent(this, typeof(OperationByCategoryActivity));
                intent.PutExtra("CategoryId", categorySum.CategoryId);
                intent.PutExtra("CategoryName", categorySum.CategoryName);
                intent.PutExtra("OperationType", operationType);
                string startPeriod = startRange.ToString("o");
                string endPeriod = endRange.ToString("o");
                intent.PutExtra("StartRange", startPeriod);
                intent.PutExtra("EndRange", endPeriod);
                StartActivity(intent);
            };
        }

        /// <summary>
        /// Показать диалог выбора периорда.
        /// </summary>
        private void ShowPeriodTypeDialog()
        {
            string[] options = new string[]
            {
                "День",
                "Неделя",
                "Месяц",
                "Год",
                "Все время"
            };

            var builder = new Android.App.AlertDialog.Builder(this);
            builder.SetTitle("Выберите период");
            builder.SetItems(options, (sender, args) =>
            {
                PeriodType selectedType;

                switch (args.Which)
                {
                    case 0:
                        selectedType = PeriodType.Day;
                        break;
                    case 1:
                        selectedType = PeriodType.Week;
                        break;
                    case 2:
                        selectedType = PeriodType.Month;
                        break;
                    case 3:
                        selectedType = PeriodType.Year;
                        break;
                    case 4:
                        selectedType = PeriodType.AllTime;
                        break;
                    default:
                        Toast.MakeText(this, "Неверный выбор", ToastLength.Short).Show();
                        return;
                }

                currentPeriodType = selectedType;
                UpdateCurrentDateToStartOfSelectedPeriod();
                UpdatePeriodDisplay();
                LoadAndDisplaySumByPeriod();
            });
            builder.Show();
        }

        /// <summary>
        /// Изменение периода.
        /// </summary>
        /// <param name="delta"></param>
        private void ChangePeriod(int delta)
        {
            switch (currentPeriodType)
            {
                case PeriodType.Day:
                    currentDate = currentDate.AddDays(delta);
                    break;
                case PeriodType.Week:
                    currentDate = currentDate.AddDays(7 * delta);
                    break;
                case PeriodType.Month:
                    currentDate = currentDate.AddMonths(delta);
                    break;
                case PeriodType.Year:
                    currentDate = currentDate.AddYears(delta);
                    break;
                case PeriodType.AllTime:
                    break;
            }
            
            UpdateCurrentDateToStartOfSelectedPeriod();
            UpdatePeriodDisplay();
            LoadAndDisplaySumByPeriod();
        }
        
        /// <summary>
        /// Изменение даты начала периода.
        /// </summary>
        private void UpdateCurrentDateToStartOfSelectedPeriod()
        {
            switch (currentPeriodType)
            {
                case PeriodType.Day:
                    currentDate = DateTime.Now.Date;
                    break;
                case PeriodType.Week:
                    int diffToMonday = ((int)currentDate.DayOfWeek + 6) % 7;
                    currentDate = currentDate.AddDays(-diffToMonday).Date;
                    break;
                case PeriodType.Month:
                    currentDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                    break;
                case PeriodType.Year:
                    currentDate = new DateTime(currentDate.Year, 1, 1);
                    break;
                case PeriodType.AllTime:
                    break;
            }
        }
        
        /// <summary>
        /// Получение текущего периода.
        /// </summary>
        /// <returns></returns>
        private DateTime GetStartOfCurrentPeriod()
        {
            switch (currentPeriodType)
            {
                case PeriodType.Day:
                    return currentDate.Date;
                case PeriodType.Week:
                    int diffToMonday = ((int)currentDate.DayOfWeek + 6) % 7;
                    return currentDate.AddDays(-diffToMonday).Date;
                case PeriodType.Month:
                    return new DateTime(currentDate.Year, currentDate.Month, 1);
                case PeriodType.Year:
                    return new DateTime(currentDate.Year, 1, 1);
                default:
                    return DateTime.MinValue;
            }
        }
        
        /// <summary>
        /// Изменение отображение периода.
        /// </summary>
        private void UpdatePeriodDisplay()
        {
            string titleText = "";
            string displayText = "";

            switch (currentPeriodType)
            {
                case PeriodType.Day:
                    titleText = "День";
                    var dayStart = GetStartOfCurrentPeriod();
                    displayText = dayStart.ToString("dd.MM.yyyy");
                    break;
                case PeriodType.Week:
                    titleText = "Неделя";
                    var weekStart = GetStartOfCurrentPeriod();
                    var weekEnd = weekStart.AddDays(6);
                    displayText = $"{weekStart.ToString("dd.MM")} - {weekEnd.ToString("dd.MM")}";
                    break;
                case PeriodType.Month:
                    titleText = "Месяц";
                    var monthStart = GetStartOfCurrentPeriod();
                    displayText = monthStart.ToString("MMMM yyyy");
                    break;
                case PeriodType.Year:
                    titleText = "Год";
                    var yearStart = GetStartOfCurrentPeriod();
                    displayText = yearStart.ToString("yyyy");
                    break;
                case PeriodType.AllTime:
                    titleText = "Все время";
                    displayText = "";
                    break;
                default:
                    titleText = "";
                    displayText = "";
                    break;
            }
            textViewPeriodTitle.Text = titleText;
            textViewCurrentPeriodDisplay.Text = displayText;
        }

        /// <summary>
        /// Загрузка отображения сумм операций по категории.
        /// </summary>
        private void LoadAndDisplaySumByPeriod()
        {
            categorySums.Clear();

            startRange = DateTime.MinValue;
            endRange = DateTime.MaxValue;

            if (currentPeriodType != PeriodType.AllTime)
            {
                startRange = GetStartOfCurrentPeriod();

                switch (currentPeriodType)
                {
                    case PeriodType.Day:
                        endRange = startRange.AddDays(1).AddSeconds(-1);
                        break;
                    case PeriodType.Week:
                        endRange = startRange.AddDays(6);
                        break;
                    case PeriodType.Month:
                        endRange = new DateTime(startRange.Year, startRange.Month, 1).AddMonths(1).AddSeconds(-1);
                        break;
                    case PeriodType.Year:
                        endRange = new DateTime(startRange.Year, 12, 31, 23, 59, 59);
                        break;
                }
            }

            List<CategorySum> sums = null;
            if (database == null)
            {
                database = new AppDBManager();
            }

            switch (operationType)
            {
                case "Доход":
                    var incomeOperation = new IncomeOperation(database);
                    sums = incomeOperation.GetOperationsSumByCategoryAndPeriod(startRange, endRange);
                    break;
                case "Расход":
                    var expenseOperation = new ExpenseOperation(database);
                    sums = expenseOperation.GetOperationsSumByCategoryAndPeriod(startRange, endRange); 
                    break;
                default:
                    Toast.MakeText(this, "Неверный тип операции", ToastLength.Short).Show();
                    return;
            }

            if (sums != null)
            {
                categorySums.AddRange(sums);
                adapter = new ArrayAdapter<CategorySum>(this, Android.Resource.Layout.SimpleListItem1, categorySums);
                listViewCategoriesSum.Adapter = adapter;
                adapter.NotifyDataSetChanged();
            }
            else
            {
                Toast.MakeText(this, "Нет данных для отображения", ToastLength.Short).Show();
            } 
        }
    }