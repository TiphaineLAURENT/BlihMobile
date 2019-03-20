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
	public partial class ShhKeysPage : ContentPage
	{
		public ShhKeysPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasBackButton(this, false);
            var list = RequestBlih.RequestList(Constants.Username, Constants.Password, "sshkey/list");
            if (list == null)
                DisplayAlert("Error: listing sshkeys failed", Constants.Reason, "OK");
            else
            {
                KeyListView.ItemsSource = list;
                KeyListView.ItemSelected += OnItemSelected;
            }
        }

        void Update(object sender, EventArgs e)
        {
            var list = RequestBlih.RequestList(Constants.Username, Constants.Password, "sshkey/list");
            if (list == null)
                DisplayAlert("Error: updating sshkeys list failed", Constants.Reason, "OK");
            else
                KeyListView.ItemsSource = list;
        }

        async void ChangePage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RepositoriesPage());
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var SelectedItem = e.SelectedItem as String;
            if (e != null)
            {
                var action = await DisplayActionSheet("Dou you want to delete " + SelectedItem, "No", "Yes");
                if (action == "Yes")
                    RequestBlih.SSHKeyDelete(Constants.Username, Constants.Password, SelectedItem);
                var list = RequestBlih.RequestList(Constants.Username, Constants.Password, "sshkey/list");
                if (list == null)
                    await DisplayAlert("Error: reupdating sshkeys list failed", Constants.Reason, "OK");
                else
                    KeyListView.ItemsSource = list;
            }
        }
    }
}