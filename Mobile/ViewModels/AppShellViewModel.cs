namespace Mobile.ViewModels;

public class AppShellViewModel : BaseViewModel
{

    private string userPhone;
    private string userNmae;

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
