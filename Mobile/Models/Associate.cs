namespace Mobile.Models;

public class Associate
{
    public string CPF { get; set; }
    public string Name { get; set; }
    public long Contact { get; set; }
    public College College { get; set; }
    public string PaymentFrequency { get; set; }
    public long Telegram { get; set; }
}
