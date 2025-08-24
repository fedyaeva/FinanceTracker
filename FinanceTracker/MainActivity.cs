namespace FinanceTracker;

[Activity(Label = "@string/app_name", MainLauncher = true)]
public class MainActivity : Activity
{
    private AppDB database;
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        
        database = new AppDB();

        // Set our view from the "main" layout resource
        SetContentView(Resource.Layout.activity_main);
    }
}