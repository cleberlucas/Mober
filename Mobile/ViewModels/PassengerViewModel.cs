using Mobile.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Mobile.ViewPopups;
using CommunityToolkit.Maui.Views;

using System.Web.WebPages;
using Mobile.Models;

namespace Mobile.ViewModels;

public class PassengerViewModel : BaseViewModel
{

    private Passenger _selectedPassenger;

    public ObservableCollection<Passenger> Passengers { get; }
    public Command RefreshPassengerCommand { get; }
    public Command AddPassengerCommand { get; }
    public Command<Passenger> PassengerTapped { get; }

    public PassengerViewModel()
    {
        Passengers = new ObservableCollection<Passenger>();

        RefreshPassengerCommand = new Command(async () => await ExecuteRefreshPassengerCommand());

        PassengerTapped = new Command<Passenger>(OnPassengerSelected);
    }

    private enum Control
    {
        Gone,
        Back
    }

    private enum Presence
    {
        Zero,
        Pending,
        Confirmed,
        ConfirmedNotListed
    }

    public async Task<bool> ExecuteRefreshPassengerCommand()
    {
        string Token = await SecureStorage.Default.GetAsync("Token");

        IsBusy = true;
        await Task.Run(async () =>
        {

            try
            {
                Passengers.Clear();

                Passenger passenger = new Passenger();
                passenger.DateTime = DateTime.Now;

                passenger.Period = PeriodDataStore.GetObject();

                PassengerDataStore.SetObjects(await PassengerDataStore.GetObjectsAsync(Token, passenger));


                if (AppShellViewModel.tabIndex == (int)Control.Gone)
                {
                    foreach (var passenger_ in PassengerDataStore.GetObjects().Where(x => !x.PresenceGone.IsEmpty()))
                    {
                        Passengers.Add(LoadColor(passenger_, (int)Control.Gone));
                    }


                }
                else if (AppShellViewModel.tabIndex == (int)Control.Back)
                {
                    foreach (var passenger_ in PassengerDataStore.GetObjects().Where(x => !x.PresenceBack.IsEmpty()))
                    {
                        Passengers.Add(LoadColor(passenger_, (int)Control.Back));
                    }

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                var m = ex.Message.ToString();
            }
            finally
            {
                IsBusy = false;

            }

        });
        return true;
    }

    public async void OnAppearing()
    {

        IsBusy = await ExecuteRefreshPassengerCommand();
        SelectedPassenger = null;

    }

    public Passenger LoadColor(Passenger passenger, int control)
    {

        if (control == (int)Control.Gone)
        {
            if (passenger.PresenceGone == Dictionary.SqlColumn.Presence.Pending) passenger.Color = Colors.LightGray;
            else if (passenger.PresenceGone == Dictionary.SqlColumn.Presence.Confirmed) passenger.Color = Colors.Lime;
            else if (passenger.PresenceGone == Dictionary.SqlColumn.Presence.NotListed) passenger.Color = Colors.Yellow;
        }
        else if (control == (int)Control.Back)
        {
            if (passenger.PresenceBack == Dictionary.SqlColumn.Presence.Pending) passenger.Color = Colors.LightGray;
            else if (passenger.PresenceBack == Dictionary.SqlColumn.Presence.Confirmed) passenger.Color = Colors.Lime;
            else if (passenger.PresenceBack == Dictionary.SqlColumn.Presence.NotListed) passenger.Color = Colors.Yellow;
        }

        return passenger;
    }

    public Passenger SelectedPassenger
    {
        get => _selectedPassenger;
        set
        {
            SetProperty(ref _selectedPassenger, value);
            OnPassengerSelected(value);
        }
    }

    async void OnPassengerSelected(Passenger passenger)
    {
        if (passenger is null) return;
        await Shell.Current.ShowPopupAsync(new PassengerDetailPopup(passenger));
    }
}