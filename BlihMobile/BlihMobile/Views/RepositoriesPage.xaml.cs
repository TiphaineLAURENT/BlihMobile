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
            var list = RequestBlih.Request(Constants.Username, Constants.Password, "repository/list");
            TextView.Text = list;
		}

        void Update(object sender, EventArgs e)
        {
            var list = RequestBlih.Request(Constants.Username, Constants.Password, "repository/list");
            TextView.Text = list;
        }

        async void ChangePage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShhKeysPage());
        }
    }
}