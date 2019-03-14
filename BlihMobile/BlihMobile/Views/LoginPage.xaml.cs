using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BlihMobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlihMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public static NavigationPage NavPage;

        public LoginPage()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            BackgroundColor = Constants.BackGroundColorLoginPage;
            ActivitySpinner.IsVisible = false;

            Entry_Username.Completed += (s, e) => Entry_Password.Focus();
            Entry_Password.Completed += (s, e) => SingInProcedure(s, e);
        }

        async void SingInProcedure(object sender, EventArgs e)
        {
            ActivitySpinner.IsVisible = true;
            User user = new User(Entry_Username.Text, Entry_Password.Text);
            Constants.Username = Entry_Username.Text;
            Constants.Password = Entry_Password.Text;
            if (user.CheckLogs())
            {
                ActivitySpinner.IsVisible = false;
                if (Device.RuntimePlatform.Equals("Android"))
                {
                    Application.Current.MainPage = new NavigationPage(new RepositoriesPage());
                }
                else if (Device.RuntimePlatform.Equals("iOS"))
                {
                    await Navigation.PushModalAsync(new NavigationPage(new RepositoriesPage()));
                }
            }
            else
            {
                await DisplayAlert("Login", "Login Failed, username or password is empty", "Ok");
            }
        }
    }
}