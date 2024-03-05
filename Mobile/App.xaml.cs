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
            MainPage = new AppShell();

            _moberLoginDataStorage.SetObject(
                new MoberLogin()
                {
                    Name = SecureStorage.Default.GetAsync("EntryName").Result ?? "",
                    Phone = SecureStorage.Default.GetAsync("EntryTelephone").Result ?? "",
                    Servant = (SecureStorage.Default.GetAsync("IsServant").Result ?? "") == "Prestador",
                }
            );
        }
        else
        {
            MainPage = new LoginView();
        }
    }
}
