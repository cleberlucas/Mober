namespace Mobile.ViewModels;

public class ServantViewModel : BaseViewModel
{

    private bool enable;
    private IEnumerable<string> _services;

    public ServantViewModel()
    {
    }

    public ServantViewModel(IEnumerable<string> services)
    {
        _services = services;
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


}
