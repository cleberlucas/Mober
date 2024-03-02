using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Mobile.Models;
public interface IPassengerDataStore<T> where T : Passenger
{
    Task<List<T>> GetObjectsAsync(string t, Passenger t2);
    List<T> GetObjects();
    T GetObject();
    void SetObjects(List<T> t);
    void SetObject(T t);
}

