
using Mobile.Interfaces;
using Mobile.ViewModels;
using Mobile.Views;

namespace Mobile;

public partial class AppShell : Shell
{
    public IMoberLoginDataStorage _moberLoginDataStorage => DependencyService.Get<IMoberLoginDataStorage>();

    AppShellViewModel _binds;

    public AppShell()
    {
        InitializeComponent();

        BindingContext = _binds = new AppShellViewModel()
        {
            UserName = _moberLoginDataStorage.GetObject().Name,
            UserPhone = _moberLoginDataStorage.GetObject().Phone,
        };

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    private void OnLogoutButtonClicked(object sender, EventArgs e)
    {
        SecureStorage.RemoveAll();
        App.Current.MainPage = new LoginView();
    }
}
