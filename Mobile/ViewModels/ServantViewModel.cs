namespace Mobile.ViewModels;

public class ServantViewModel : BaseViewModel
{

    private bool enable;
    private string userNmae;
    private string userPhone;

    private IEnumerable<string> _services;

    public ServantViewModel()
    {
    }

    public IEnumerable<string> Services
    {
        get => _services;
        set => SetProperty(ref _services, value);
    }

    public bool Enable
    {
        get => enable;
        set => SetProperty(ref enable, value);
    }

    public string UserName
    {
        get => userNmae;
        set => SetProperty(ref userNmae, value);
    }

    public string UserPhone
    {
        get => userPhone;
        set => SetProperty(ref userPhone, value);
    }
}


