using Android.Content;

namespace FinanceTracker;

[Activity(Label = "SelectCategoryActivity")]
public class SelectCategoryActivity : Activity
{
    private ListView listViewCategories;
    private EditText editTextNewCategory;
    private Button buttonAddCategory, buttonBack;
    private TextView textViewTitle;

    private List<string> categories = new List<string>();
    private ArrayAdapter<string> adapter;

    private string operationType;

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.select_category);

        listViewCategories = FindViewById<ListView>(Resource.Id.listViewCategories);
        editTextNewCategory = FindViewById<EditText>(Resource.Id.editTextNewCategory);
        buttonAddCategory = FindViewById<Button>(Resource.Id.buttonAddCategory);
        buttonBack = FindViewById<Button>(Resource.Id.buttonBack);
        textViewTitle = FindViewById<TextView>(Resource.Id.textViewTitle);

        operationType = Intent.GetStringExtra("operationType") ?? "Доход";

        textViewTitle.Text = operationType == "Расход" ? "Категория расхода" : "Категория дохода";

        LoadCategories();

        adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItemSingleChoice, categories);
        listViewCategories.Adapter = adapter;
        listViewCategories.ChoiceMode = ChoiceMode.Single;

        listViewCategories.ItemClick += (sender, e) =>
        {
            string selectedCategory = categories[e.Position];

            var resultIntent = new Intent();
            resultIntent.PutExtra("SelectedCategory", selectedCategory);
            SetResult(Result.Ok, resultIntent);
            Finish();
        };

        listViewCategories.ItemLongClick += (s, e) =>
        {
            ShowContextMenu(e.Position);
            e.Handled = true;
        };

        buttonAddCategory.Click += (s, e) =>
        {
            var newCatName = editTextNewCategory.Text.Trim();
            if (!string.IsNullOrEmpty(newCatName))
            {
                if (!categories.Contains(newCatName))
                {
                    bool success = AddCategoryToDB(newCatName);
                    if (success)
                    {
                        categories.Add(newCatName);
                        adapter.NotifyDataSetChanged();
                        editTextNewCategory.Text = "";
                        Toast.MakeText(this, "Категория добавлена", ToastLength.Short).Show();
                    }
                    else
                    {
                        Toast.MakeText(this, "Ошибка при добавлении категории", ToastLength.Short).Show();
                    }
                }
                else
                {
                    Toast.MakeText(this, "Такая категория уже существует", ToastLength.Short).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "Введите название категории", ToastLength.Short).Show();
            }
        };

        buttonBack.Click += (s, e) => Finish();
    }

    protected override void OnResume()
    {
        base.OnResume();
        LoadCategories();
    }

    private void LoadCategories()
    {
        categories.Clear();

        AppDBManager database = new AppDBManager();

        if (operationType == "Доход")
        {
            var incomeCatManager= new IncomeCategory(database);
            var incomeCats= incomeCatManager.GetCategories();
            foreach (var cat in incomeCats)
                categories.Add(cat.Name);
        }
        else if (operationType == "Расход")
        {
            var expenseCatManager= new ExpenseCategory(database);
            var expenseCats= expenseCatManager.GetCategories();
            foreach (var cat in expenseCats)
                categories.Add(cat.Name);
        }
        adapter?.NotifyDataSetChanged();
    }

    private void ShowContextMenu(int position)
    {
        string categoryName= categories[position];

        string[] options= { "Удалить", "Редактировать" };

        var builder= new AlertDialog.Builder(this);
        builder.SetItems(options, (sender, args) =>
        {
            int which= args.Which;
            if (which==0)
                ConfirmAndDeleteCategory(position);
            else if (which==1)
                ShowEditCategoryDialog(position);
        });
        builder.Show();
    }

    private void ConfirmAndDeleteCategory(int position)
    {
        string categoryName= categories[position];

        var alertBuilder= new AlertDialog.Builder(this);
        alertBuilder.SetMessage($"Вы уверены, что хотите удалить '{categoryName}'?");
        alertBuilder.SetPositiveButton("Да", (senderAlert, args) =>
        {
            bool success= DeleteCategoryFromDB(categoryName);
            if(success)
            {
                categories.RemoveAt(position);
                adapter.NotifyDataSetChanged();
                Toast.MakeText(this, "Категория удалена", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Ошибка при удалении категории", ToastLength.Short).Show();
            }
        });
        alertBuilder.SetNegativeButton("Нет", (sender, args) => { });
        alertBuilder.Show();
    }

    private void ShowEditCategoryDialog(int position)
    {
       string oldName= categories[position];

       var inputEditText= new EditText(this){ Text= oldName };

       var dialogBuilder= new AlertDialog.Builder(this);
       dialogBuilder.SetTitle("Редактировать категорию");
       dialogBuilder.SetView(inputEditText);

       dialogBuilder.SetPositiveButton("Сохранить", (senderAlert, args) =>
       {
           string newName= inputEditText.Text.Trim();

           if(!string.IsNullOrEmpty(newName))
           {
               if(!categories.Contains(newName))
               {
                   bool success= RenameCategoryInDB(oldName,newName);

                   if(success)
                   {
                       categories[position]= newName;
                       adapter.NotifyDataSetChanged();
                       Toast.MakeText(this,"Категория обновлена",ToastLength.Short).Show();
                   }
                   else
                   {
                       Toast.MakeText(this,"Ошибка при обновлении категории",ToastLength.Short).Show();
                   }
               }
               else
               {
                   Toast.MakeText(this,"Такая категория уже существует",ToastLength.Short).Show();
               }
           }
           else
           {
               Toast.MakeText(this,"Имя не может быть пустым",ToastLength.Short).Show();
           }
       });
       dialogBuilder.SetNegativeButton("Отмена", (sender, args)=>{});
       dialogBuilder.Show();
   }
    
   private bool AddCategoryToDB(string categoryName)
   {
       try
       {
           AppDBManager database= new AppDBManager();

           if(operationType=="Доход")
           {
               var incomeCatManager= new IncomeCategory(database);
               incomeCatManager.AddCategory(categoryName);
           }
           else if(operationType=="Расход")
           {
               var expenseCatManager= new ExpenseCategory(database);
               expenseCatManager.AddCategory(categoryName);
           }
           return true;
       }
       catch
       {
           return false;
       }
   }

   private bool DeleteCategoryFromDB(string categoryName)
   {
       try
       {
           AppDBManager database= new AppDBManager();

           if(operationType=="Доход")
           {
               var incomeCatManager= new IncomeCategory(database);

               var categoryToDelete= incomeCatManager.GetCategories().FirstOrDefault(c=>c.Name==categoryName);

               if(categoryToDelete != null)
                   incomeCatManager.RemoveCategory(categoryToDelete.Id); 
           }
           else if(operationType=="Расход")
           {

               var expenseCatManager= new ExpenseCategory(database);

               var categoryToDelete= expenseCatManager.GetCategories().FirstOrDefault(c=>c.Name==categoryName);

               if(categoryToDelete != null)
                   expenseCatManager.RemoveCategory(categoryToDelete.Id); 
           }
           return true;
       }
       catch
       { 
          return false; 
       }
   }

   private bool RenameCategoryInDB(string oldName, string newName)
   {
       try
       {
           AppDBManager database= new AppDBManager();

           if(operationType=="Доход")
           {
               var incomeCatManager= new IncomeCategory(database);
               var category= incomeCatManager.GetCategories().FirstOrDefault(c=>c.Name==oldName);

               if(category != null)
               {
                   incomeCatManager.RenameCategory(category.Id, newName);
                   return true;
               }
           }
           else if(operationType=="Расход")
           {
               var expenseCatManager= new ExpenseCategory(database);
               var category= expenseCatManager.GetCategories().FirstOrDefault(c=>c.Name==oldName);

               if(category != null)
               {
                   expenseCatManager.RenameCategory(category.Id, newName);
                   return true;
               }
           }
       }
       catch (Exception ex)
       {
           return false;
       }

       return false;
   }
}