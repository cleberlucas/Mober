
using Mobile.Models;
using Mobile.WebClient;
using System.Reflection;

namespace Mobile.DataStore;

public class ProfileDataStore : IProfileDataStore<Profile>
{
    private Profile _profile;

    public Profile GetObject()
    {
        return _profile;
    }

    public void SetObject(Profile profile)
    {
        _profile = profile;
    }

    public async Task<Profile> GetObjectAsync(string token)
    {
        return await new ProfileWebClient(token).View();
    }
}

