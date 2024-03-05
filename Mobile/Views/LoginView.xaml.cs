using Mobile.Interfaces;
using Mobile.Models;
using System.Web.WebPages;


namespace Mobile.Views;

public partial class LoginView : ContentPage
{
    public IMoberLoginDataStorage _moberLoginDataStorage => DependencyService.Get<IMoberLoginDataStorage>();

    public LoginView()
    {
        InitializeComponent();
    }


    private async void OnPickerTypeSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        try
        {
            ActivityIndicator_Load.IsRunning = true;

            EntryName.Text = EntryName.Text.TrimEnd();
            EntryTelephone.Text = EntryTelephone.Text.TrimEnd();

            if (EntryName.Text.IsEmpty())
            {
                await DisplayAlert("Dados inválidos!", "Informe seu nome!", "OK");
            }
            else
            {
                if (EntryTelephone.Text.IsEmpty())
                {
                    await DisplayAlert("Dados inválidos!", "Informe seu telefone!", "OK");
                }
                else
                {
                    if (((string)PickerServant.SelectedItem).IsEmpty())
                    {
                        await DisplayAlert("Dados inválidos!", "Informe se você é um Contratante/Prestador!", "OK");
                    }
                    else
                    {
                        await SecureStorage.Default.SetAsync("EntryName", EntryName.Text);
                        await SecureStorage.Default.SetAsync("EntryTelephone", EntryTelephone.Text);
                        await SecureStorage.Default.SetAsync("IsServant", (string)PickerServant.SelectedItem);

                        _moberLoginDataStorage.SetObject(
                            new MoberLogin()
                            {
                                Name = EntryName.Text,
                                Phone = EntryTelephone.Text,
                                Servant = ((string)PickerServant.SelectedItem) == "Prestador",
                            }
                        );

                        await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                        App.Current.MainPage = new AppShell();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ocorreu um erro inesperado", ex.Message, "OK");
        }

        finally
        {
            ActivityIndicator_Load.IsVisible = false;
        }
    }


}

