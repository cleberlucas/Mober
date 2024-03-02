using Mobile.ViewPopups;
using Mobile.Views;
using Mobile.Models;
using Mobile.ViewModels;
using System.Reflection;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Mobile;

public partial class AppShell : Shell
{
    AppShellViewModel _binds;

    Profile _profile;

    public IProfileDataStore<Profile> profileDataStore => DependencyService.Get<IProfileDataStore<Profile>>();

    public AppShell()
	{       
        InitializeComponent();
       
        Routing.RegisterRoute(nameof(PassengerDetailPopup), typeof(PassengerDetailPopup));
        Routing.RegisterRoute(nameof(ScannerPage), typeof(ScannerPage));
    }

    protected async override void OnAppearing() {
        base.OnAppearing();

        //profileDataStore.SetObject(
        //    await profileDataStore.GetObjectAsync(await SecureStorage.Default.GetAsync("Token"))
        //);

        //_profile = profileDataStore.GetObject();

        //_binds = new AppShellViewModel();

        //_binds.Logo = _profile.Logo;
        //_binds.Associatedescription = _profile.Description;
        //_binds.Associatename = _profile.Name;

        //BindingContext = _binds;
    }




    private void OnLogoutButtonClicked(object sender, EventArgs e)
    {
        SecureStorage.RemoveAll();
        App.Current.MainPage = new LoginView();
    }

    private void ChangeTabIndexTo0(object sender, EventArgs e)
    {
        AppShellViewModel.tabIndex = 0;    
    }

    private void ChangeTabIndexTo1(object sender, EventArgs e)
    {
        AppShellViewModel.tabIndex = 1;
    }
}
