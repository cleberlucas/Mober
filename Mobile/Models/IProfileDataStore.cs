using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Mobile.Models;
public interface IProfileDataStore<T> where T : Profile
{
    Task<T> GetObjectAsync(string token);
    void SetObject(T t);
    T GetObject();
}

