using AppProjectGym.Information;
using AppProjectGym.Models;

namespace AppProjectGym.Pages;

public partial class ProfilePage : ContentPage
{
    private User user;

    public ProfilePage()
    {
        InitializeComponent();
        BindingContext = this;
        User = ClientInfo.User;
    }

    public User User
    {
        get => user;
        private set
        {
            user = value;
            OnPropertyChanged();
        }
    }
}