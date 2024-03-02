using System.Web.Mvc;

namespace Mobile.Models;

public interface IVehicleWebClient<T> where T : Vehicle
{
    Task<List<T>> List();
    Task<bool> UpdateLocation(T t);
}


