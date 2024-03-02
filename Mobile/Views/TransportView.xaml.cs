
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Maps;
using Mobile.Models;
using Mobile.ViewModels;
using Mobile.WebClient;
using System.Reflection;
using System.Threading.Tasks;

namespace Mobile.Views;



public partial class TransportView : ContentPage
{
    public IPointDataStore<Mobile.Models.Point> PointDataStore => DependencyService.Get<IPointDataStore<Mobile.Models.Point>>();
    public IPeriodDataStore PeriodDataStore => DependencyService.Get<IPeriodDataStore>();
    public IVehicleDataStore<Vehicle> VehicleDataStore => DependencyService.Get<IVehicleDataStore<Vehicle>>();

    private CancellationTokenSource _cancelTokenSource;
    public bool _liveLocation;
    public string _token;

    public enum PointDirections
    {
        Next,
        Previous
    }

    TransportViewModel _viewModel;

    public TransportView()
    {
        InitializeComponent();

        //Task.Run (() => LoadData().Wait()).Wait();

        //BindingContext = _viewModel = new TransportViewModel(
        //    VehicleDataStore.GetObjects(),
        //    PeriodDataStore.GetObjects()
        //);

        _viewModel = new ();
    }

    public void LoadData()
    {
        //_token = await SecureStorage.Default.GetAsync("Token");

        //VehicleDataStore.SetObjects(
        //    await VehicleDataStore.GetObjectsAsync(_token)
        //);

        PeriodDataStore.SetObjects(
            new List<string>()
            {
                Dictionary.SqlColumn.Period.Morning,
                Dictionary.SqlColumn.Period.Afternoon,
                Dictionary.SqlColumn.Period.Evening
            }
        );

        //PointDataStore.SetObjects(
        //    await PointDataStore.GetObjectsAsync(_token)
        //);
    }

   

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        LoadData();

        await Task.Run(() => InitializeMapComponent());
        await Task.Run(() => ServiceLiveLocation());
    }

    public async Task InitializeMapComponent()
    {
        try
        {
            while (!_viewModel.Enable)
            {
                await Task.Delay(1000);

                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(1));

                _cancelTokenSource = new CancellationTokenSource();

                var location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(location.Latitude, location.Longitude), Distance.FromMiles(1)));
                    _viewModel.Enable = true;
                });

            }
        } catch (Exception ex) {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Ocorreu um erro ao inicializar o mapa:", ex.Message, "OK");
            });
        }     
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        string period = Picker.SelectedItem as string;

        if (period is null) return;


        _viewModel.Title = period;

        PeriodDataStore.SetObject(period);

        map.Pins.Clear();

        if (PointDataStore.GetObjects() is null) return;
        foreach (var point in PointDataStore.GetObjects().Where(x => x.Period == period))
        {
            map.Pins.Add(new Pin
            {
                Label = point.Id.ToString(),
                Address = point.Description,
                Location = new Location(point.Latitude, point.Longitude)

            });
        }
    }

    private async void LiveLocactionButton_Cliked(object sender, EventArgs e)
    {
        _liveLocation = !_liveLocation;
        
        if (_liveLocation)
        {
            Frame.BackgroundColor = Colors.Yellow;
        }
        else
        {

            Frame.BackgroundColor = Colors.White;
            Frame.BorderColor = Colors.White;
        }

    }

    public async Task ServiceLiveLocation()
    {
        var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

        _cancelTokenSource = new CancellationTokenSource();

        var location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

        //var vehicle = VehicleDataStore.GetObject();

        Frame.BackgroundColor = Colors.Yellow;

        while (_liveLocation)
        {
            Frame.BackgroundColor = Colors.LimeGreen;
            //if (vehicle is not null)
            //{
            //    vehicle.Latitude = location.Latitude;
            //    vehicle.Longitude = location.Longitude;
            //    if (await new VehicleWebClient(_token).UpdateLocation(vehicle))
            //        Frame.BackgroundColor = Colors.LimeGreen;
            //    else 
            //        Frame.BackgroundColor = Colors.Yellow;
            //}
            await Task.Delay(10000);
        }
     
    }

}

