
using Mobile.Models;
using Mobile.WebClient;

public class VehicleDataStore : IVehicleDataStore<Vehicle> {
    private List<Vehicle> _vehicles;
    private Vehicle _vehicle;

    public Vehicle GetObject() {
        return _vehicle;
    }

    public List<Vehicle> GetObjects() {
        return _vehicles;
    }

    public async Task<List<Vehicle>> GetObjectsAsync(string token) {
        return await new VehicleWebClient(token).List();
    }

    public void SetObject(Vehicle vehicle) {
        _vehicle = vehicle;
    }

    public void SetObjects(List<Vehicle> vehicles) {
        _vehicles = vehicles;
    }
}