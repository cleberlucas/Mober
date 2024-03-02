using Microsoft.Maui.ApplicationModel.Communication;
using Mobile.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace Mobile.ViewModels;

public class TransportViewModel : BaseViewModel
{

    private bool enable;
    private IEnumerable<Vehicle> _vehicles;
    private IEnumerable<string> _periods;

    public TransportViewModel()
    {
    }

    public TransportViewModel(IEnumerable<Vehicle> vehicles, IEnumerable<string> periods)
    {
        _vehicles = vehicles;
        _periods = periods;
    }

    public IEnumerable<Vehicle> Vehicles
    {
        get => _vehicles;
        set => SetProperty(ref _vehicles, value);
    }

    public IEnumerable<string> Periods
    {
        get => _periods;
        set => SetProperty(ref _periods, value);
    }

    public bool Enable
    {
        get => enable;
        set => SetProperty(ref enable, value);
    }


}
