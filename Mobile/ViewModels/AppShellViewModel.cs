namespace Mobile.ViewModels;

public class AppShellViewModel : BaseViewModel
{
    public static int tabIndex;

    private string logo;
    private string associatedescription;
    private string associatename;
    public string Logo
    {
        get => logo;
        set => logo = value;
    }
    public string Associatedescription
    {
        get => associatedescription;
        set => associatedescription = value;
    }
    public string Associatename
    {
        get => associatename;
        set => associatename = value;
    }
}
