
using Appwrite.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Mobile.Externals;
using Mobile.Models;
using Mobile.Repositories;
using Mobile.ViewModels;
using System.Web.WebPages;

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

        ServicesDataStore.SetObjects(Services.List);

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

                await Task.Delay(1000);
            }
            catch (Exception ex)
            {
                //MainThread.BeginInvokeOnMainThread(async () =>
                //{
                //    await DisplayAlert("Ocorreu um erro ao inicializar o mapa:", ex.Message, "OK");
                //});

                await Task.Delay(10000);

            }
        }
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        map.Pins.Clear();
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
            var db = new MoberContext(new DbContextOptions<MoberContext>());
            var user = new User();

            Frame.BackgroundColor = Colors.Yellow;

            _serviceLiveLocationRunning = true;

            Frame.BackgroundColor = Colors.LimeGreen;
            while (_liveLocation)
            {
                var service = Picker.SelectedItem as string;

                if (service.IsEmpty())
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await DisplayAlert("Serviço não informado:", "Você deve informar um serviço!", "OK");
                    });

                    Frame.BackgroundColor = Colors.Red;

                    _serviceLiveLocationRunning = false;

                    return;
                }


                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                var location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                var name = await SecureStorage.Default.GetAsync("EntryName");
                var phone = await SecureStorage.Default.GetAsync("EntryTelephone");

                user.Name = name;
                user.Phone = phone;
                user.Latitude = location.Latitude;
                user.Longitude = location.Longitude;
                user.Service = service;
                user.Servant = true;
                user.Updated = DateTime.Now;

                var appwriteService = new AppwriteService();

                if ((await appwriteService.GetUser(name)).Name.IsEmpty())
                {
                    await appwriteService.CreateUser(user);
                }
                else
                {
                    await appwriteService.UpdateUser(user);
                }


                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    map.Pins.Clear();
                    foreach (var item in (await appwriteService.GetUsers()).Where(x => x.Servant == false && x.Service == service))
                    {
                        
                        map.Pins.Add(new Pin
                        {
                            Label = $" {await SecureStorage.Default.GetAsync("EntryName")} -  {await SecureStorage.Default.GetAsync("EntryTelephone")} ",
                            Location = new Location(location.Latitude, location.Longitude)

                        });
                    }

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

