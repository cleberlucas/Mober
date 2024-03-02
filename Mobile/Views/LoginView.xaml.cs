
using Mobile.Models;
using Mobile.WebClient;
using System.Web.WebPages;


namespace Mobile.Views;

public partial class LoginView : ContentPage
{
    public IProfileDataStore<Profile> profileDataStore => DependencyService.Get<IProfileDataStore<Profile>>();

    public LoginView() {     
        InitializeComponent();
    }

    protected async override void OnAppearing()
    {
        EntryName.Text = await SecureStorage.Default.GetAsync("EntryName");
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        try 
        {
            ActivityIndicator_Load.IsRunning = true;

            if (EntryName.Text.IsEmpty()) {
                await DisplayAlert("Dados inválidos!", "É necessário informar seu nome!", "OK");
            } else {

                await SecureStorage.Default.SetAsync("EntryName", EntryName.Text);

                await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                await Permissions.RequestAsync<Permissions.Camera>();

                App.Current.MainPage = new AppShell();
            }
        }
        catch (Exception ex) 
        {
            await DisplayAlert("Ocorreu um erro inesperado", ex.Message, "OK"); 
        }
            
        finally {
            ActivityIndicator_Load.IsVisible = false;
        }
    }


}

