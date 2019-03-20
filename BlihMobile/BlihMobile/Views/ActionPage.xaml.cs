using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using BlihMobile.Models;
using System.Windows.Input;

namespace BlihMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ActionPage : ContentPage
	{

        public ActionPage ()
		{
			InitializeComponent ();
            Title = Constants.SelectedRepo;
            UpdateList();
        }

        void UpdateList()
        {
            Constants.AclUsers = RequestBlih.RequestAcl(Constants.Username, Constants.Password, Constants.SelectedRepo);
            if (Constants.AclUsers == null)
                DisplayAlert("Error: updating acl list failed", Constants.Reason, "OK");
            else
                AclListView.ItemsSource = Constants.AclUsers;
        }

        async void AddUser(object sender, EventArgs e)
        {
            string myinput = await InputBox(this.Navigation);
            bool returnV = RequestBlih.SetAcl(Constants.Username, Constants.Password, Constants.SelectedRepo, myinput, "r");
            if (returnV == false)
                await DisplayAlert("Error: adding acl user failed", Constants.Reason, "OK");
            UpdateList();
        }

        async void ShowValue(object sender, EventArgs e)
        {
            foreach (var user in Constants.AclUsers)
            {
                string acl = "";

                if (user.read == true)
                    acl += "r";
                if (user.write == true)
                    acl += "w";
                if (user.execute == true)
                    acl += "x";

                bool returnV = RequestBlih.SetAcl(Constants.Username, Constants.Password, Constants.SelectedRepo, user.name, acl);
                if (returnV == false)
                    await DisplayAlert("Error: adding acl user failed", Constants.Reason, "OK");
            }
        }

        async void DeleteRepo(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Are you sure you want to delete " + Constants.SelectedRepo + " ?", "No", "Yes");
            if (action == "Yes")
            {
                bool returnV = RequestBlih.ManageRepo(Constants.Username, Constants.Password, Constants.SelectedRepo, "repository/delete");
                if (returnV == false)
                    await DisplayAlert("Error: deleting repository failed", Constants.Reason, "OK");
            }
            await Navigation.PushAsync(new RepositoriesPage());
        }

        public static Task<string> InputBox(INavigation navigation)
        {
            // wait in this proc, until user did his input 
            var tcs = new TaskCompletionSource<string>();

            var lblTitle = new Label { Text = "Enter the email", HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold };
            var lblMessage = new Label { Text = "By default ACL will be read only" };
            var txtInput = new Entry { Text = "" };

            var btnOk = new Button
            {
                Text = "Ok",
                WidthRequest = 100,
                BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8),
            };
            btnOk.Clicked += async (s, e) =>
            {
                // close page
                var result = txtInput.Text;
                await navigation.PopModalAsync();
                // pass result
                tcs.SetResult(result);
            };

            var btnCancel = new Button
            {
                Text = "Cancel",
                WidthRequest = 100,
                BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8)
            };
            btnCancel.Clicked += async (s, e) =>
            {
                // close page
                await navigation.PopModalAsync();
                // pass empty result
                tcs.SetResult(null);
            };

            var slButtons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { btnOk, btnCancel },
            };

            var layout = new StackLayout
            {
                Padding = new Thickness(0, 40, 0, 0),
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { lblTitle, lblMessage, txtInput, slButtons },
            };

            // create and show page
            var page = new ContentPage();
            page.Content = layout;
            navigation.PushModalAsync(page);
            // open keyboard
            txtInput.Focus();

            // code is waiting her, until result is passed with tcs.SetResult() in btn-Clicked
            // then proc returns the result
            return tcs.Task;
        }
    }
}