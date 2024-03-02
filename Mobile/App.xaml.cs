using Mobile.Models;
using Mobile.Views;
using System.Web.WebPages;

namespace Mobile;
public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        DependencyService.Register<IServiceDataStore, PeriodDataStore>();

        if (!SecureStorage.Default.GetAsync("EntryName").Result.IsEmpty()) MainPage = new AppShell();
        else MainPage = new LoginView();
    }
}
