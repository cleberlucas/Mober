using CommunityToolkit.Maui.Views;
using Mobile.Models;
using Mobile.ViewModels;
using System.ComponentModel;

namespace Mobile.ViewPopups;

public partial class PassengerDetailPopup : Popup
{
    public PassengerDetailPopup(Passenger passenger)
    {
        InitializeComponent();
        BindingContext = new PassengerDetailViewModel()
        {
            CPF = passenger.Associate.CPF,
            College = passenger.Associate.College.Description,
            Name = passenger.Associate.Name,
            Contact = passenger.Associate.Contact
        };
    }

    async private void ImageButton_wpp_Clicked(object sender, System.EventArgs e)
    {
        await Browser.OpenAsync("https://api.whatsapp.com/send?phone=" + ImageButton_wpp.CommandParameter.ToString());
    }

    private void ImageButton_call_Clicked(object sender, System.EventArgs e)
    {
        PhoneDialer.Open(ImageButton_wpp.CommandParameter.ToString());
    }
}