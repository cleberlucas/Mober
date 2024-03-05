using Mobile.Models;

namespace Mobile.Interfaces;
public interface IMoberLoginDataStorage
{
    public MoberLogin GetObject();
    public void SetObject(MoberLogin moberLogin);
}
