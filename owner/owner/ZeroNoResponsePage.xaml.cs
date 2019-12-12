using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ZeroNoResponsePage : ContentPage
	{
		public ZeroNoResponsePage (string title)
		{
			InitializeComponent ();

            lbl_title.Text = title;

            if (App.owner_type == "1")
            {
                btn_upgrade.IsVisible = false;
            }
		}

        private async void Back_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Btn_upgrade_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyInfoUpdatePage());
        }
    }
}