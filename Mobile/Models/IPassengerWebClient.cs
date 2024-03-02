using Mobile.Models;
using System.Web.Mvc;

public interface IPassengerWebClient<T> where T : Passenger
{
    Task<List<T>> List(Passenger t2);
    Task<bool> ConfirmPresenceGone(Passenger t);
    Task<bool> ConfirmPresenceBack(Passenger t);
    Task<bool> Reserve(Passenger t);
}


