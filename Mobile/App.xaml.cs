using Mobile.Views;
using System.Web.WebPages;

using Mobile.DataStore;
using Mobile.Models;

namespace Mobile;
public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        DependencyService.Register<IProfileDataStore<Profile>, ProfileDataStore>();
        DependencyService.Register<IVehicleDataStore<Vehicle>, VehicleDataStore>();
        DependencyService.Register<IPointDataStore<Mobile.Models.Point>, PointDataStore>();
        DependencyService.Register<IPassengerDataStore<Passenger>,PassengerDataStore>();    
        DependencyService.Register<IPeriodDataStore,PeriodDataStore>();    
        
        if (!SecureStorage.Default.GetAsync("EntryName").Result.IsEmpty()) MainPage = new AppShell();
        else MainPage = new LoginView();
    }
}
