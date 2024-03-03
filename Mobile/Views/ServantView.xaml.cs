﻿using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Mobile.DataStore.Externals;
using Mobile.Models;
using Mobile.ViewModels;
using System.Web.WebPages;

namespace Mobile.Views;



public partial class ServantView : ContentPage
{
    public IServiceDataStore ServicesDataStore => DependencyService.Get<IServiceDataStore>();

    private CancellationTokenSource _cancelTokenSource;

    AppwriteService _appwriteService;

    bool _serviceLiveLocationRunning;
    public bool _liveLocation;
    public string _token;
    public string _name;
    public string _phone;


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

        _appwriteService = new AppwriteService();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        _name = await SecureStorage.Default.GetAsync("EntryName");
        _phone = await SecureStorage.Default.GetAsync("EntryTelephone");

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
            var service = Picker.SelectedItem as string;

            if (service.IsEmpty())
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
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                var location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                user.Name = _name;
                user.Phone = _phone;
                user.Latitude = location.Latitude;
                user.Longitude = location.Longitude;
                user.Service = service;
                user.Servant = true;
                user.Updated = DateTime.Now;

                var appwriteService = new AppwriteService();

                if ((await appwriteService.GetUser(_name)).Name.IsEmpty())
                {
                    await appwriteService.CreateUser(user);
                }
                else
                {
                    await appwriteService.UpdateUser(user);
                }

                var contractors = (await _appwriteService.GetUsers())
                    .Where(
                        x => x.Servant == false &&
                        x.Service == service &&
                        DateTime.Now - x.Updated < TimeSpan.FromMinutes(5)
                    );

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    map.Pins.Clear();
                    foreach (var contractor in contractors)
                    {

                        map.Pins.Add(new Pin
                        {
                            Label = $"Contratante: {contractor.Name} \n Telefone: {contractor.Phone} \n Avaliação: {contractor.Rate}",
                            Location = new Location(location.Latitude, location.Longitude)

                        });
                    }
                });


                if  (Frame.BackgroundColor != Colors.LimeGreen) Frame.BackgroundColor = Colors.LimeGreen; ;
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

