using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Mobile.Models;
public interface IPeriodDataStore
{
    List<string> GetObjects();
    string GetObject();
    void SetObjects(List<string> t);
    void SetObject(string t);
}

