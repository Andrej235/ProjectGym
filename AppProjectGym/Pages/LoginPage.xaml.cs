using AppProjectGym.Information;
using AppProjectGym.Services;
using System.Diagnostics;

namespace AppProjectGym.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void OnLoginSubmited(object sender, EventArgs e)
    {
        if (await ClientInfo.Login(emailInputField.Text, passwordInputField.Text))
            await NavigationService.GoToAsync("..");
        else
            Debug.WriteLine("---> Invalid login info");
    }
}