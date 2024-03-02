using System.Web.Mvc;

namespace Mobile.Models;

public interface IPointWebClient<T> where T : Point
{
    Task<List<T>> ListToday();
}



