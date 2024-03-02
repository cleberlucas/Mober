using Mobile.Models;
using Mobile.ViewModels;
using Mobile.WebClient;

namespace Mobile.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ScannerPage : ContentPage
{
    public IPassengerDataStore<Passenger> PassengerDataStore => DependencyService.Get<IPassengerDataStore<Passenger>>();
    public IPeriodDataStore PeriodDataStore => DependencyService.Get<IPeriodDataStore>();

    ScannerViewModel _viewModel;
    string Token;
    string antSpan="";
    string code = "";
    bool antSpanValidate;

    Passenger passenger;

    public ScannerPage()
    {
        InitializeComponent();
        BindingContext = _viewModel = new ScannerViewModel();
        Token = SecureStorage.Default.GetAsync("Token").Result;
    }
 
    protected override void OnAppearing()
    {     
        base.OnAppearing();          
        _viewModel.OnAppearing(); 
    }

    protected async override void OnDisappearing() { await Navigation.PopAsync(); }


    private async void CameraBarcodeReaderView_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        antSpanValidate = true;
        code = e.Results[0].Value.ToString();

        if (antSpan == code || code.Length != 11 || !code.All(char.IsDigit))
            return;

        await Task.Run(async () =>
        {
            passenger = PassengerDataStore.GetObjects().FirstOrDefault(x => x.Associate.CPF == code);

            if (passenger is null)
            {
                MainThread.BeginInvokeOnMainThread(() => Image_statusQrCode.Source = "scan_scanning");

                passenger = new Passenger();
                passenger.DateTime = DateTime.Now;
                passenger.Period = PeriodDataStore.GetObject();

                PassengerDataStore.SetObjects(await PassengerDataStore.GetObjectsAsync(Token, passenger));
                passenger = PassengerDataStore.GetObjects().FirstOrDefault(x => x.Associate.CPF == code);

                if (passenger is null)
                {
                    passenger = new Passenger();
                    passenger.DateTime = DateTime.Now;
                    passenger.Period = PeriodDataStore.GetObject();

                    passenger.Associate = new Associate();
                    passenger.Associate.CPF = code;
                    passenger.Associate.Contact = 0;

                    passenger.Point = new Models.Point();
                    passenger.Point.Id = 9999;

                    if (await new PassengerWebClient(Token).Reserve(passenger))
                    {
                        MainThread.BeginInvokeOnMainThread(() => Image_statusQrCode.Source = "scan_true");
                        antSpanValidate = false;
                    }
                        
                    else
                        MainThread.BeginInvokeOnMainThread(() => Image_statusQrCode.Source = "scan_false");
                    await Task.Delay(1000);

                }

                MainThread.BeginInvokeOnMainThread(() => Image_statusQrCode.Source = "scan_void");
            }

            if (AppShellViewModel.tabIndex == 0)
            {
                if (await new PassengerWebClient(Token).ConfirmPresenceGone(passenger))
                {
                    MainThread.BeginInvokeOnMainThread(() => Image_statusQrCode.Source = "scan_true");
                    await Task.Delay(1000);
                    _viewModel.Passengers.Remove(passenger);
                    passenger.Color = Colors.White;
                    _viewModel.Passengers.Add(passenger);
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(() => Image_statusQrCode.Source = "scan_false");
                    await Task.Delay(1000);
                }
            }

            if (AppShellViewModel.tabIndex == 1)
            {
                if (await new PassengerWebClient(Token).ConfirmPresenceBack(passenger))
                {
                    MainThread.BeginInvokeOnMainThread(() => Image_statusQrCode.Source = "scan_true");
                    await Task.Delay(1000);
                    _viewModel.Passengers.Remove(passenger);
                    passenger.Color = Colors.White;
                    _viewModel.Passengers.Add(passenger);
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(() => Image_statusQrCode.Source = "scan_false");
                    await Task.Delay(1000);
                }
            }         
        });


        if (antSpanValidate)
            antSpan = code;
        MainThread.BeginInvokeOnMainThread(() => Image_statusQrCode.Source = "scan_void");

    }

    public void Entry_CPF_Completed(object sender, EventArgs e)
    {
  
        //passenger = _viewModel.Passenger.FirstOrDefault(x => x.CPF == Entry_CPF.Text);

        //if (passenger is not null || Entry_CPF.Text.Length != 11 || !Entry_CPF.Text.All(char.IsDigit)) return;

        //_ = Task.Run( () =>
        //{
        //    passenger = MySQLService.GetPassenger(Entry_CPF.Text);
        //    passenger.StatusColor = Colors.WhiteSmoke;


        //    MainThread.BeginInvokeOnMainThread(async () => 
        //    {
        //        if (await DisplayAlert("", $"Deseja confirmar {passenger.Name}?", "Confirmar", "Cancelar"))
        //        {


        //            _viewModel.Passenger.Add(passenger);
        //            if (MySQLService.UpdatePassenger(Entry_CPF.Text,  AppShellViewModel.tabIndex == 0 ? "gone" : "back", transport.Period.ToString()))
        //            {
        //                _viewModel.Passenger.Remove(passenger);
        //                passenger.StatusColor = Color.FromRgba("B9FF86");
        //                _viewModel.Passenger.Add(passenger);
        //            }

        //            else 
        //                if (MySQLService.UpdatePassenger2(Entry_CPF.Text, AppShellViewModel.tabIndex == 0 ? "gone" : "back", transport.Period.ToString()) ||
        //                     MySQLService.InsertPassenger(Entry_CPF.Text, AppShellViewModel.tabIndex == 0 ? "gone" : "back", transport.Period.ToString()))
        //                {
        //                    _viewModel.Passenger.Remove(passenger);
        //                    passenger.StatusColor = Color.FromRgba("FFFA86");
        //                    _viewModel.Passenger.Add(passenger);
        //                }
        //                else
        //                {
        //                    _viewModel.Passenger.Remove(passenger);
        //                    passenger.StatusColor = Color.FromRgba("FF8F86");
        //                    _viewModel.Passenger.Add(passenger);
        //                }
        //        }
        //    });
          
        //});
        

           
    }

 
}


//SALVAR PONTO ATUAL