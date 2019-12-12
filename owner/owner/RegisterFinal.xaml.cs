using owner.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterFinal : ContentPage
	{
		public RegisterFinal ()
		{
			InitializeComponent ();

            NavigationPage.SetHasNavigationBar(this, false);
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.White;
            ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.Black;

            var lblTapFinance = new TapGestureRecognizer();
            lblTapFinance.Tapped += (s, e) =>
            {
                Step_final_next_btn_Clicked(s, e);
            };

            lbl_register_final_later.GestureRecognizers.Add(lblTapFinance);
        }

        private async void Step_final_next_btn_Clicked(object sender, EventArgs e)
        {
            Preferences.Set(Constants.ALLOW_PUSH, true);
            await Navigation.PushAsync(new RegisterComplete
            {

            });
        }

        private async void Back_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}