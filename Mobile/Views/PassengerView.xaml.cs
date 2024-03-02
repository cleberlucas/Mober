using Mobile.ViewModels;

namespace Mobile.Views;
public partial class PassengerPage : ContentPage
{
    PassengerViewModel _viewModel;

    public PassengerPage()
    {
        InitializeComponent();
        BindingContext = _viewModel = new PassengerViewModel();
    }

    protected override void OnAppearing()
    {          
        base.OnAppearing();
        _viewModel.OnAppearing();        
    }

    private async  void ImageButton(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ScannerPage());
    }
}