using Mobile.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace Mobile.ViewModels;

public class ScannerViewModel : BaseViewModel
{

    public ObservableCollection<Passenger> Passengers { get; }
    public Command RefreshPassengerCommand { get; }
    public Command ConfirmPassenger { get; }

    public ScannerViewModel()
    {
        Title = "ASSOCIADOS";
        Passengers = new ObservableCollection<Passenger>();
    }


    public void OnAppearing()
    {
        IsBusy = Passengers.Count == 0;
    }

}