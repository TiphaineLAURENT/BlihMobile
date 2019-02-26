using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlihMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RepositoriesPage : ContentPage
	{
		public RepositoriesPage ()
		{
			InitializeComponent ();
		}

        void Update(object sender, EventArgs e)
        {
        }

        async void ChangePage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShhKeysPage());
        }
    }
}