namespace FinanceTracker;

[Activity(Label = "@string/app_name", MainLauncher = true)]
public class MainActivity : Activity
{
    private AppDBManager database;
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        
        database = new AppDBManager();

        // Set our view from the "main" layout resource
        SetContentView(Resource.Layout.activity_main);
    }
}