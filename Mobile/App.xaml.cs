using Mobile.Interfaces;
using Mobile.Models;
using Mobile.Views;
using System.Web.WebPages;

namespace Mobile;
public partial class App : Application
{
    public IMoberLoginDataStorage _moberLoginDataStorage => DependencyService.Get<IMoberLoginDataStorage>();
    public App()
    {
        InitializeComponent();
        DependencyService.Register<IMoberLoginDataStorage, MoberLoginDataStorage>();


        if (!SecureStorage.Default.GetAsync("EntryName").Result.IsEmpty())
        {
            _moberLoginDataStorage.SetObject(
                new MoberLogin()
                {
                    Name = SecureStorage.Default.GetAsync("EntryName").Result ?? "",
                    Phone = SecureStorage.Default.GetAsync("EntryTelephone").Result ?? "",
                }
            );

            MainPage = new AppShell();
        }
        else
        {
            MainPage = new LoginView();
        }
    }
}
