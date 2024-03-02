namespace Mobile.Models;
public interface IServiceDataStore
{
    List<string> GetObjects();
    string GetObject();
    void SetObjects(List<string> t);
    void SetObject(string t);
}

