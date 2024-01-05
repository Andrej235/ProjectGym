using AppProjectGym.Information;
using AppProjectGym.Services;
using System.Diagnostics;

namespace AppProjectGym.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        #region LogIn
        private void OnSwitchToLogin(object sender, EventArgs e)
        {
            registerWrapper.IsVisible = false;
            loginWrapper.IsVisible = true;
        }

        private async void OnSubmitLoginInfo(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(loginInputField_Email.Text)
                || !loginInputField_Email.Text.Contains('@')
                || string.IsNullOrEmpty(loginInputField_Password.Text)
                || loginInputField_Password.Text.Length < 8)
            {
                Debug.WriteLine("---> Invalid login info");
                wrongLoginDetailsLabel.IsVisible = true;
            }

            if (await ClientInfo.Login(loginInputField_Email.Text, loginInputField_Password.Text))
            {
                await NavigationService.GoToAsync("..");
            }
            else
            {
                Debug.WriteLine("---> Invalid login info");
                wrongLoginDetailsLabel.IsVisible = true;
            }
        }
        #endregion

        #region Register
        private void OnSwitchToRegister(object sender, EventArgs e)
        {
            loginWrapper.IsVisible = false;
            registerWrapper.IsVisible = true;
        }

        private async void OnSubmitRegisterInfo(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(registerInputField_Name.Text))
            {
                registerMissingLabel_Name.IsVisible = true;
                return;
            }
            else
                registerMissingLabel_Name.IsVisible = false;

            if (string.IsNullOrEmpty(registerInputField_Email.Text))
            {
                registerMissingLabel_Email.IsVisible = true;
                return;
            }
            else
                registerMissingLabel_Email.IsVisible = false;

            if (string.IsNullOrEmpty(registerInputField_Password.Text) || registerInputField_Password.Text.Length < 8)
            {
                registerMissingLabel_Password.IsVisible = true;
                return;
            }
            else
                registerMissingLabel_Password.IsVisible = false;

            if (passwordsDontMatchLabel.IsVisible)
                return;

            if (await ClientInfo.Register(registerInputField_Name.Text, registerInputField_Email.Text, registerInputField_Password.Text))
            {
                await NavigationService.GoToAsync("..");
            }
            else
            {
                Debug.WriteLine("---> Invalid register info");
                wrongLoginDetailsLabel.IsVisible = true;
            }
        }

        private void OnRegisterPasswordInputChanged(object sender, TextChangedEventArgs e) => passwordsDontMatchLabel.IsVisible = registerInputField_Password.Text != registerInputField_RepeatPassword.Text;
        #endregion

        #region Custom back button logic
        protected override bool OnBackButtonPressed()
        {
            BackCommand.Execute(null);
            return true;
        }

        public Command BackCommand => new(() =>
        {
            return;
        });
        #endregion
    }
}