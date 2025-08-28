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
                        categories.Add(newCatName);
                        SaveCategories();
                        adapter.NotifyDataSetChanged();
                        editTextNewCategory.Text = "";
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

        private void LoadCategories()
        {
            AppDBManager database = new AppDBManager();
            
           categories.Clear();

           if (operationType == "Доход")
           {
               var incomeCatManager = new IncomeCategory(database);
               var incomeCats = incomeCatManager.GetCategories();
               foreach (var cat in incomeCats)
               {
                   categories.Add(cat.Name);
               }
           }
           else if (operationType == "Расход")
           {
               var expenseCatManager = new ExpenseCategory(database);
               var expenseCats = expenseCatManager.GetCategories(); 
               foreach (var cat in expenseCats)
               {
                   categories.Add(cat.Name);
               }
           }
           adapter?.NotifyDataSetChanged();
        }

        private void ShowContextMenu(int position)
        {
            string categoryName = categories[position];

            string[] options = { "Удалить", "Редактировать" };
            
            var builder = new AlertDialog.Builder(this);
            
            builder.SetItems(options, (sender, args) =>
            {
                int which = args.Which;
                if (which == 0)
                {
                    ConfirmAndDeleteCategory(position);
                }
                else if (which == 1)
                {
                    ShowEditCategoryDialog(position);
                }
            });
            
             builder.Show();
        }

        private void ConfirmAndDeleteCategory(int position)
        {
             string categoryName = categories[position];

             var alertBuilder = new AlertDialog.Builder(this);
             alertBuilder.SetMessage($"Вы уверены, что хотите удалить '{categoryName}'?");
             alertBuilder.SetPositiveButton("Да", (senderAlert, args) =>
             {
                 DeleteCategoryFromDB(categoryName);
                 categories.RemoveAt(position);
                 SaveCategories();
                 adapter.NotifyDataSetChanged();
             });
             alertBuilder.SetNegativeButton("Нет", (sender, args) => { });
             alertBuilder.Show();
         }

         private void ShowEditCategoryDialog(int position)
         {
             string oldName = categories[position];

             var inputEditText = new EditText(this) { Text = oldName };

             var dialogBuilder = new AlertDialog.Builder(this);
             dialogBuilder.SetTitle("Редактировать категорию");
             dialogBuilder.SetView(inputEditText);

             dialogBuilder.SetPositiveButton("Сохранить", (senderAlert, args) =>
             {
                 string newName = inputEditText.Text.Trim();
                 if (!string.IsNullOrEmpty(newName))
                 {
                     if (!categories.Contains(newName))
                     {
                         RenameCategoryInDB(oldName, newName);
                         categories[position] = newName;
                         SaveCategories();
                         adapter.NotifyDataSetChanged();
                     }
                     else
                     {
                         Toast.MakeText(this, "Такая категория уже существует", ToastLength.Short).Show();
                     }
                 }
                 else
                 {
                     Toast.MakeText(this, "Имя не может быть пустым", ToastLength.Short).Show();
                 }
             });
             dialogBuilder.SetNegativeButton("Отмена", (sender, args) => { });
             dialogBuilder.Show();
         }

         private void SaveCategories()
         {
             AppDBManager database = new AppDBManager();

             if (operationType == "Доход")
             {
                 var incomeCatManager = new IncomeCategory(database);

                 foreach (var name in categories)
                 {
                     try
                     {
                         incomeCatManager.AddCategory(name);
                     }
                     catch
                     {
                         // Можно оставить пустым или логировать ошибку
                     }
                 }
             }
             else if (operationType == "Расход")
             {
                 var expenseCatManager = new ExpenseCategory(database);

                 foreach (var name in categories)
                 {
                     try
                     {
                         expenseCatManager.AddCategory(name);
                     }
                     catch
                     {
                         // Обработка ошибок при добавлении
                     }
                 }
             }
         }
         private void DeleteCategoryFromDB(string categoryName)
         {
             AppDBManager database = new AppDBManager();
             
             if (operationType == "Доход")
             {
                  var incomeCatManager= new IncomeCategory(database);

                  var categoryToDelete= incomeCatManager.GetCategories().FirstOrDefault(c => c.Name==categoryName);

                  if(categoryToDelete != null)
                  {
                      incomeCatManager.RemoveCategory(categoryToDelete.Id); 
                  }
             }
             else if(operationType=="Расход")
             {

                  var expenseCatManager= new ExpenseCategory(database);

                  var categoryToDelete= expenseCatManager.GetCategories().FirstOrDefault(c => c.Name==categoryName);

                  if(categoryToDelete != null)
                  {
                      expenseCatManager.RemoveCategory(categoryToDelete.Id); 
                  }

             }
         }

         private void RenameCategoryInDB(string oldName, string newName)
         {
             AppDBManager database = new AppDBManager();

              if(operationType=="Доход")
              {

                   var incomeCatManager= new IncomeCategory(database);

                   var category= incomeCatManager.GetCategories().FirstOrDefault(c=>c.Name==oldName);

                   if(category != null)
                   {

                       category.Name= newName;

                       try{incomeCatManager.RenameCategory();} catch{}
                   }

              }else if(operationType=="Расход")
              {

                   var expenseCatManager= new ExpenseCategory(database);

                   var category= expenseCatManager.GetCategories().FirstOrDefault(c=>c.Name==oldName);

                   if(category != null)
                   {

                       category.Name= newName;

                       try{expenseCatManager.RenameCategory();} catch{}
                   }

              }

         }
}