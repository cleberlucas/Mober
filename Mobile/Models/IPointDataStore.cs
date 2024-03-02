using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Mobile.Models;
public interface IPointDataStore<T> where T : Point
{
    Task<List<T>> GetObjectsAsync(string t);
    List<T> GetObjects();
    T GetObject();
    void SetObjects(List<T> t);
    void SetObject(T t);
}

