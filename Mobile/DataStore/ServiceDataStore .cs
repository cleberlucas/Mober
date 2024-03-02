using Mobile.Models;

public class PeriodDataStore : IServiceDataStore
{
    private List<string> _periods;
    private string _period;

    public string GetObject()
    {
        return _period;
    }

    public List<string> GetObjects()
    {
        return _periods;
    }

    public void SetObject(string period)
    {
        _period = period;
    }

    public void SetObjects(List<string> periods)
    {
        _periods = periods;
    }
}