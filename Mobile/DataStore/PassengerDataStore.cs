
using Mobile.Models;
using Mobile.WebClient;

namespace Mobile.DataStore;

public class PassengerDataStore : IPassengerDataStore<Passenger>
{
    private List<Passenger> _passengers;
    private Passenger _passenger;

    public Passenger GetObject()
    {
        return _passenger;
    }

    public List<Passenger> GetObjects()
    {
        return _passengers;
    }

    public async Task<List<Passenger>> GetObjectsAsync(string token, Passenger passenger)
    {
        return await new PassengerWebClient(token).List(passenger);
    }

    public void SetObject(Passenger passenger)
    {
        _passenger = passenger;
    }

    public void SetObjects(List<Passenger> passenger)
    {
        _passengers = passenger;
    }
}