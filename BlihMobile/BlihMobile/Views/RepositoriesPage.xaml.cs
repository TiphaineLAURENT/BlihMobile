using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using BlihMobile.Models;

namespace BlihMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RepositoriesPage : ContentPage
    {
		public RepositoriesPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasBackButton(this, false);
            var list = RequestBlih.RequestList(Constants.Username, Constants.Password, "repository/list");
            if (list == null)
                DisplayAlert("Error: list repositories list failed", Constants.Reason, "OK");
            else
            {
                RepoListView.ItemsSource = list;
                RepoListView.ItemSelected += OnItemSelected;
            }
        }

        void Update(object sender, EventArgs e)
        {
            var list = RequestBlih.RequestList(Constants.Username, Constants.Password, "repository/list");
            if (list == null)
                DisplayAlert("Error: update repositories list failed", Constants.Reason, "OK");
            else
                RepoListView.ItemsSource = list;
        }

        async void ChangePage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShhKeysPage());
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Constants.SelectedRepo = e.SelectedItem as String;
            if (e != null)
            {
                await Navigation.PushAsync(new ActionPage());
            }
        }

        async void AddRepo(object sender, EventArgs e)
        {
            string myinput = await InputBox(this.Navigation);
            bool returnV = RequestBlih.ManageRepo(Constants.Username, Constants.Password, myinput, "repository/create");
            if (returnV == false)
                await DisplayAlert("Error: create repository failed", Constants.Reason, "OK");
            var list = RequestBlih.RequestList(Constants.Username, Constants.Password, "repository/list");
            if (list == null)
                await DisplayAlert("Error: reupdate repositories list failed", Constants.Reason, "OK");
            else
                RepoListView.ItemsSource = list;
        }

        public static Task<string> InputBox(INavigation navigation)
        {
            // wait in this proc, until user did his input 
            var tcs = new TaskCompletionSource<string>();

            var lblTitle = new Label { Text = "Enter repository name", HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold };
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
                Children = { lblTitle, txtInput, slButtons },
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