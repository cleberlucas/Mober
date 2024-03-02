using System.Diagnostics;


namespace Mobile.ViewModels;

[QueryProperty(nameof(CPF), nameof(CPF))]
public class PassengerDetailViewModel : BaseViewModel
{
    private string name;
    private string college;
    private long contact;
    private string datepay;
    private string modpay;
    private string rg;
    private string cpf;


    public string Name
    {
        get => name;
        set => SetProperty(ref name, value);
    }

    public string College
    {
        get => college;
        set => SetProperty(ref college, value);
    }

    public long Contact
    {
        get => contact;
        set => SetProperty(ref contact, value);
    }

    public string CPF
    {
        get
        {
            return cpf;
        }
        set
        {
            SetProperty(ref cpf, value);
        }
    }

    public string DatePay
    {
        get => datepay;
        set => SetProperty(ref datepay, value);
    }

    public string ModPay
    {
        get => modpay;
        set => SetProperty(ref modpay, value);
    }

    public string RG
    {
        get => rg;
        set => SetProperty(ref rg, value);
    }
}
