namespace Mobile.Models;
public interface IMoberLoginDataStorage
{
    public MoberLogin GetObject();
    public void SetObject(MoberLogin moberLogin);
}
