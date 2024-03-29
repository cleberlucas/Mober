﻿using Microsoft.Maui.Maps;
using Mobile.Externals;
using Mobile.Interfaces;
using Mobile.Models;
using Mobile.ViewModels;
using System.Web.WebPages;
using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace Mobile.Views;



public partial class ContractorView : ContentPage
{
    public IMoberLoginDataStorage _moberLoginDataStorage => DependencyService.Get<IMoberLoginDataStorage>();

    private CancellationTokenSource _cancelTokenSource;

    AppwriteService _appwriteService;
    MoberUser _user;

    bool _serviceLiveLocationRunning;
    public bool _liveLocation;
    public string _token;

    readonly ContractorViewModel _viewModel;

    public ContractorView()
    {
        InitializeComponent();


        BindingContext = _viewModel = new ContractorViewModel()
        {
            Services = Services.List
        };

        _user = new ();

        _user.Name = _moberLoginDataStorage.GetObject().Name;
        _user.Phone = _moberLoginDataStorage.GetObject().Phone;
        _user.Servant = false;

        _appwriteService = new();
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
            }
            catch
            {

            }

            await Task.Delay(10000);
        }
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {

        map.Pins.Clear();
    }

    private async void LiveLocactionButton_Cliked(object sender, EventArgs e)
    {
        if (((string)PickerService.SelectedItem).IsEmpty())
        {
            await DisplayAlert("Serviço não informado:", "Você deve informar um serviço!", "OK");
            return;
        }

        _liveLocation = !_liveLocation;


        if (_liveLocation)
        {
            if (!_serviceLiveLocationRunning) await Task.Run(() => ServiceLiveLocation());
        }
        else
        {
            Frame.BackgroundColor = Colors.White;
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

            _serviceLiveLocationRunning = true;

            while (_liveLocation)
            {
                MainThread.BeginInvokeOnMainThread(() => { ActivityIndicatorLoading.IsRunning = true; });

                _user.Service = (string)PickerService.SelectedItem;
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                _cancelTokenSource = new CancellationTokenSource();

                _user.Latitude = location.Latitude;
                _user.Longitude = location.Longitude;
                _user.Updated = DateTime.Now;

                if ((await _appwriteService.GetUser(_user.Name)).Name.IsEmpty())
                {
                    await _appwriteService.CreateUser(_user);
                }
                else
                {
                    await _appwriteService.UpdateUser(_user);
                }

                var contractors = (await _appwriteService.GetUsers())
                    .Where(
                        x =>
                        x.Servant != _user.Servant &&
                        x.Service == _user.Service &&
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
                            Location = new Location(contractor.Latitude, contractor.Longitude),
                            Address = $"Avaliação: {contractor.Rate}",
                            ImageSource = ImageSource.FromFile("person.png")
                        });

                    }
                });


                if (Frame.BackgroundColor != Colors.MediumSpringGreen) Frame.BackgroundColor = Colors.MediumSpringGreen;

                MainThread.BeginInvokeOnMainThread(() => ActivityIndicatorLoading.IsRunning = false);
                await Task.Delay(10000);

                Frame.BackgroundColor = Colors.White;
            }

        }
        catch (Exception ex)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Ocorreu um erro no serviço da localização:", ex.Message, "OK");
            });

            Frame.BackgroundColor = Colors.Red;
            MainThread.BeginInvokeOnMainThread(() => ActivityIndicatorLoading.IsRunning = false);
        }
        finally
        {
            _serviceLiveLocationRunning = false;
        }
    }

}

