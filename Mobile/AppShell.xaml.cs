
using Mobile.ViewModels;
using Mobile.Views;

namespace Mobile;

public partial class AppShell : Shell
{
    AppShellViewModel _binds;

    public AppShell()
    {
        InitializeComponent();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
    }

    private void OnLogoutButtonClicked(object sender, EventArgs e)
    {
        SecureStorage.RemoveAll();
        App.Current.MainPage = new LoginView();
    }
}
