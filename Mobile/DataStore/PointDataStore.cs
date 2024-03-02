
using Mobile.Models;
using Mobile.WebClient;

public class PointDataStore : IPointDataStore<Mobile.Models.Point> {
    private List<Mobile.Models.Point> _pointPeriods;
    private Mobile.Models.Point _pointPeriod;

    public Mobile.Models.Point GetObject()
    {
        return _pointPeriod;
    }

    public List<Mobile.Models.Point> GetObjects()
    {
        return _pointPeriods;
    }

    public async Task<List<Mobile.Models.Point>> GetObjectsAsync(string token)
    {
        return await new PointWebClient(token).ListToday();
    }

    public void SetObject(Mobile.Models.Point pointPeriod)
    {
        _pointPeriod = pointPeriod;
    }

    public void SetObjects(List<Mobile.Models.Point> pointPeriod)
    {
        _pointPeriods = pointPeriod;
    }



    //public Task SetObjects(string token) {
    //    _pointPeriods = new PointWebClient(token).List().Result;
    //    return Task.CompletedTask;
    //}

    //public List<Point> GetObjects() {
    //    return _pointPeriods;
    //}

    //public Point GetObject() {
    //    return _pointPeriod;
    //}

    //public void SetObject(Point pointPeriod) {
    //    _pointPeriod = pointPeriod;
    //}

}