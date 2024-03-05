using Android.Locations;
using Appwrite.Services;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Mobile.Externals;
using Mobile.Interfaces;
using Mobile.Models;
using Mobile.ViewModels;
using System.Web.WebPages;
using static Android.Graphics.ImageDecoder;
using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace Mobile.Views;



public partial class MapView : ContentPage
{
    public IMoberLoginDataStorage _moberLoginDataStorage => DependencyService.Get<IMoberLoginDataStorage>();

    private CancellationTokenSource _cancelTokenSource;

    AppwriteService _appwriteService;

    bool _serviceLiveLocationRunning;
    public bool _liveLocation;
    public string _token;

    public enum PointDirections
    {
        Next,
        Previous
    }

    readonly MapViewModel _viewModel;

    public MapView()
    {
        InitializeComponent();


        BindingContext = _viewModel = new MapViewModel(Services.List);
        _appwriteService = new AppwriteService();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        PickerService.Title = _moberLoginDataStorage.GetObject().Servant ? "Buscar serviço" : "Solicitar serviço";

        await Task.Run(() => InitializeMapComponent());
    }

    protected async override void OnDisappearing()
    {
    }

    public async Task InitializeMapComponent()
    {
        while (!_viewModel.Enable)
        {
            try
            {

                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(1));

                _cancelTokenSource = new CancellationTokenSource();

                var location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                if (location is not null)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        map.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(location.Latitude, location.Longitude), Distance.FromMiles(1)));
                        _viewModel.Enable = true;
                    });
                }
            }
            catch
            {
                //MainThread.BeginInvokeOnMainThread(async () =>
                //{
                //    await DisplayAlert("Ocorreu um erro ao inicializar o mapa:", ex.Message, "OK");
                //});

            }

            await Task.Delay(10000);
        }
    }



    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_liveLocation && _serviceLiveLocationRunning)
        {
            Frame.BackgroundColor = Colors.Yellow;

        }

        map.Pins.Clear();
    }


    private async void LiveLocactionButton_Cliked(object sender, EventArgs e)
    {
        _liveLocation = !_liveLocation;

        if (_liveLocation)
        {
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
            if (((string)PickerService.SelectedItem).IsEmpty())
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Serviço não informado:", "Você deve informar um serviço!", "OK");
                });

                _serviceLiveLocationRunning = false;

                return;
            }

            var user = new MoberUser();

            Frame.BackgroundColor = Colors.Yellow;

            _serviceLiveLocationRunning = true;

            while (_liveLocation)
            {
                var service = (string)PickerService.SelectedItem;

                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                var location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                user.Name = _moberLoginDataStorage.GetObject().Name;
                user.Phone = _moberLoginDataStorage.GetObject().Phone;
                user.Latitude = location.Latitude;
                user.Longitude = location.Longitude;
                user.Service = service;
                user.Servant = _moberLoginDataStorage.GetObject().Servant;
                user.Updated = DateTime.Now;

                if ((await _appwriteService.GetUser(_moberLoginDataStorage.GetObject().Name)).Name.IsEmpty())
                {
                    await _appwriteService.CreateUser(user);
                }
                else
                {
                    await _appwriteService.UpdateUser(user);
                }

                var contractors = (await _appwriteService.GetUsers())
                    .Where(
                        x =>
                        x.Servant != _moberLoginDataStorage.GetObject().Servant &&
                        x.Service == service &&
                        DateTime.Now - x.Updated < TimeSpan.FromMinutes(5)
                    );

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    map.Pins.Clear();
                    foreach (var contractor in contractors)
                    {

                        map.Pins.Add(new CustomPin
                        {
                            Label = contractor.Name,
                            Location = new Location(location.Latitude, location.Longitude),
                            Address = $"Avaliação: {contractor.Rate}",
                            //ImageSource = ImageSource.FromResource($"Mobile.Resources.Images.{(_moberLoginDataStorage.GetObject().Servant ? "contractor" : "servant")}.png")
                            ImageSource = ImageSource.FromUri(new Uri("https://img.icons8.com/arcade/64/person-male.png"))
                        });
     
                    }
                });


                if (Frame.BackgroundColor != Colors.LimeGreen) Frame.BackgroundColor = Colors.LimeGreen; ;
                await Task.Delay(10000);
            }

            Frame.BackgroundColor = Colors.White;
            Frame.BorderColor = Colors.White;

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

