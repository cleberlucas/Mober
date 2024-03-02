
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Mobile.Models;
using Mobile.ViewModels;

namespace Mobile.Views;



public partial class ServantView : ContentPage
{
    public IServiceDataStore ServicesDataStore => DependencyService.Get<IServiceDataStore>();
    

    private CancellationTokenSource _cancelTokenSource;


    bool _serviceLiveLocationRunning;
    public bool _liveLocation;
    public string _token;

    public enum PointDirections
    {
        Next,
        Previous
    }

    ServantViewModel _viewModel;

    public ServantView()
    {
        InitializeComponent();

        ServicesDataStore.SetObjects(Dictionary.Services.List);


        BindingContext = _viewModel = new ServantViewModel(
            ServicesDataStore.GetObjects()
        );

    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        await Task.Run(() => InitializeMapComponent());
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
        }
        catch (Exception ex)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Ocorreu um erro ao inicializar o mapa:", ex.Message, "OK");
            });
        }
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        //string period = Picker.SelectedItem as string;

        //if (period is null) return;


        //_viewModel.Title = period;

        //ServicesDataStore.SetObject(period);

        //map.Pins.Clear();

        //if (PointDataStore.GetObjects() is null) return;
        //foreach (var point in PointDataStore.GetObjects().Where(x => x.Period == period))
        //{
        //    map.Pins.Add(new Pin
        //    {
        //        Label = point.Id.ToString(),
        //        Address = point.Description,
        //        Location = new Location(point.Latitude, point.Longitude)

        //    });
        //}
    }

    private async void LiveLocactionButton_Cliked(object sender, EventArgs e)
    {
        _liveLocation = !_liveLocation;

        if (_liveLocation)
        {
            Frame.BackgroundColor = Colors.Yellow;
            if (!_serviceLiveLocationRunning) await Task.Run(() => ServiceLiveLocation());
        }
        else
        {

            Frame.BackgroundColor = Colors.White;
            Frame.BorderColor = Colors.White;
        }

    }

    public async Task ServiceLiveLocation()
    {
        try
        {
            //var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

            //_cancelTokenSource = new CancellationTokenSource();

            //var location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

            //var vehicle = VehicleDataStore.GetObject();

            Frame.BackgroundColor = Colors.Yellow;

            _serviceLiveLocationRunning = true;

            Frame.BackgroundColor = Colors.LimeGreen;
            while (_liveLocation)
            {

                //if (vehicle is not null)
                //{
                //    vehicle.Latitude = location.Latitude;
                //    vehicle.Longitude = location.Longitude;
                //    if (await new VehicleWebClient(_token).UpdateLocation(vehicle))
                //        Frame.BackgroundColor = Colors.LimeGreen;
                //    else 
                //        Frame.BackgroundColor = Colors.Yellow;
                //}

                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                var location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    map.Pins.Clear();
                    map.Pins.Add(new Pin
                    {
                        Label = "Labelxxx",
                        Address = "Label",
                        Location = new Location(location.Latitude, location.Longitude)

                    });
                });

                await Task.Delay(10000);
            }

        }
        catch (Exception ex)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Ocorreu um erro no serviço da localização:", ex.Message, "OK");
            });

            Frame.BackgroundColor = Colors.Red;
        }
        finally
        {
            _serviceLiveLocationRunning = false;
        }
    }

}

