using Mobile.Interfaces;
using Mobile.Models;

public class MoberLoginDataStorage : IMoberLoginDataStorage
{
    private MoberLogin _moberLogin;

    public MoberLogin GetObject()
    {
        return _moberLogin;
    }

    public void SetObject(MoberLogin moberLogin)
    {
        _moberLogin = moberLogin;
    }

}