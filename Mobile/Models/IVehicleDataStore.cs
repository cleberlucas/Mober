using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Mobile.Models;
public interface IVehicleDataStore<T> where T : Vehicle
{
    Task<List<T>> GetObjectsAsync(string t);
    List<T> GetObjects();
    T GetObject();
    void SetObjects(List<T> t);
    void SetObject(T t);
}

